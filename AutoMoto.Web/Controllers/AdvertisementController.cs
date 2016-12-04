using AutoMapper;
using AutoMoto.Contracts.Interfaces;
using AutoMoto.Contracts.ViewModels;
using AutoMoto.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutoMoto.Web.Controllers
{
    public class AdvertisementController : Controller
    {
        private readonly IManufacturerService _manufacturerService;
        private readonly IModelService _modelService;
        private readonly IAdvertisementService _advertisementService;
        private readonly IFollowingService _followingService;
        private readonly IUserNotificationService _userNotificationService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public AdvertisementController(IManufacturerService manufacturerService, IModelService modelService, IUnitOfWorkAsync unitOfWorkAsync, IAdvertisementService advertisementService, IUserNotificationService userNotificationService, IFollowingService followingService)
        {
            _manufacturerService = manufacturerService;
            _modelService = modelService;
            _advertisementService = advertisementService;
            _followingService = followingService;
            _userNotificationService = userNotificationService;
            _unitOfWorkAsync = unitOfWorkAsync;
        }



        public ActionResult Index(int? page)
        {
            int pageSize = 2;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var advertisements =
                _advertisementService.Queryable().Include(x => x.Car.Model.Manufacturer).Include(x => x.Photos).OrderByDescending(x => x.AddedDate).ToPagedList(pageIndex, pageSize); ;

            return View(advertisements);
        }

        public ActionResult Details(int id)
        {
            var advertisement = _advertisementService.Queryable().Include(x => x.Car).First(x => x.Id == id);
            var viewModel = Mapper.Map<Advertisement, AdvertisementDetailsViewModel>(advertisement);
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                viewModel.IsFollowing = _followingService.GetFollowing(userId, advertisement.UserId) != null;
            }

            return View(viewModel);
        }

        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new AdvertisementViewModel();
            viewModel.Manufacturers = _manufacturerService.Queryable().ToList();
            return View("Form", viewModel);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create(AdvertisementViewModel viewModel)
        {
            if (!ModelState.IsValid || viewModel.PhotoFiles == null)
            {
                viewModel.Manufacturers = _manufacturerService.Queryable().ToList();
                return View("Form", viewModel);
            }
            var userId = User.Identity.GetUserId();
            var car = new Car()
            {
                EngineCap = viewModel.EngineCap,
                FuelType = viewModel.FuelType,
                Mileage = viewModel.Mileage,
                ModelId = viewModel.Model,
                Price = viewModel.Price,
                Year = viewModel.Year,
                ObjectState = ObjectState.Added
            };
            List<Photo> photos = new List<Photo>();
            foreach (var photoFile in viewModel.PhotoFiles)
            {
                Photo photo = new Photo();
                using (var reader = new System.IO.BinaryReader(photoFile.InputStream))
                {
                    photo.Content = reader.ReadBytes(photoFile.ContentLength);
                }
                photo.Extension = photoFile.ContentType;
                photo.ObjectState = ObjectState.Added;
                photos.Add(photo);
            }


            var advertisement = new Advertisement()
            {
                AddedDate = DateTime.Now,
                Description = viewModel.Description,
                Car = car,
                IsActive = true,
                Title = viewModel.Title,
                UserId = userId,
                Photos = photos,
                ObjectState = ObjectState.Added

            };
            _advertisementService.InsertOrUpdateGraph(advertisement);

            var followers = _followingService.GetFollowingByFollowee(userId).Select(x => x.FollowerId).ToList();
            if (followers.Any())
            {

                var followingNotification = new FollowingNotification() { Advertisement = advertisement, DateTime = DateTime.Now, ObjectState = ObjectState.Added };
                foreach (var followeer in followers)
                {
                    var userNotification = new UserNotification() { IsRead = false, Notification = followingNotification, UserId = followeer, ObjectState = ObjectState.Added };
                    _userNotificationService.Insert(userNotification);
                }
            }

            await _unitOfWorkAsync.SaveChangesAsync();
            return RedirectToAction("Index", "Home");

        }
        public JsonResult FillModels(int manufacturer)
        {
            var models =
                _modelService.Queryable().Where(x => x.ManufacturerId == manufacturer).ToList();
            return Json(models, JsonRequestBehavior.AllowGet);
        }

    }
}