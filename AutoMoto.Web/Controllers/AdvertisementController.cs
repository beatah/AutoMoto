using AutoMoto.Contracts;
using AutoMoto.Contracts.Dtos;
using AutoMoto.Contracts.Interfaces;
using AutoMoto.Contracts.ViewModels;
using AutoMoto.Model;
using AutoMoto.Model.Models;
using AutoMoto.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
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
        private readonly IMessageService _messageService;
        private readonly IFeatureService _featureService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public AdvertisementController(IMessageService messageService, IManufacturerService manufacturerService, IModelService modelService, IUnitOfWorkAsync unitOfWorkAsync, IAdvertisementService advertisementService, IUserNotificationService userNotificationService, IFollowingService followingService, IFeatureService featureService)
        {
            _manufacturerService = manufacturerService;
            _modelService = modelService;
            _advertisementService = advertisementService;
            _followingService = followingService;
            _featureService = featureService;
            _userNotificationService = userNotificationService;
            _messageService = messageService;
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        public async Task<ActionResult> ContactUser(ContactDto model)
        {
            var advertisement = await _advertisementService.FindAsync(model.AdvertisementId);

            var userIdCurrent = User.Identity.GetUserId();



            if (advertisement.UserId == userIdCurrent)
            {
                TempData[TempKeys.UserMessageAlertState] = "bg-danger";
                TempData[TempKeys.UserMessage] = "Nie możesz wysłać wiadomości do siebie!";
                return RedirectToAction("Details", "Advertisement", new { id = model.AdvertisementId });
            }


            var message = new Message()
            {
                FromUserId = userIdCurrent,
                ToUserId = advertisement.UserId,
                Content = model.Message,
                AdvertisementId = advertisement.Id
            };

            _messageService.Insert(message);
            var messageNotification = new MessageNotification() { Message = message, DateTime = DateTime.Now, ObjectState = ObjectState.Added };

            var userNotification = new UserNotification()
            {

                IsRead = false,
                Notification = messageNotification,
                UserId = message.ToUserId,
                ObjectState = ObjectState.Added
            };
            _userNotificationService.Insert(userNotification);


            await _unitOfWorkAsync.SaveChangesAsync();


            TempData[TempKeys.UserMessage] = "Wiadomość wysłana!";

            return RedirectToAction("Details", "Advertisement", new { id = model.AdvertisementId });
        }


        public async Task<ActionResult> Index(SearchModel model)
        {
            await GetSearchResult(model);
            return View("~/Views/Advertisement/Index.cshtml", model);

        }
        public async Task<ActionResult> Mine()
        {
            var userId = User.Identity.GetUserId();
            var items = await _advertisementService.Query(x => x.UserId == userId)
                        .Include(x => x.Car)
                        .Include(x => x.Car.Model)
                        .Include(x => x.Car.Model.Manufacturer)
                        .Include(x => x.User)
                        .Include(x => x.Photos)
                        .Include(x => x.User.Address)
                        .SelectAsync();
            var itemsModelList = new List<ListingItemModel>();
            foreach (var item in items.Where(x => x.IsActive).OrderByDescending(x => x.AddedDate))
            {
                itemsModelList.Add(new ListingItemModel()
                {
                    ListingCurrent = item,
                    UrlPicture = item.Photos.Count == 0 ? "" : string.Format("data:{0};base64,{1}", item.Photos.First().Extension, Convert.ToBase64String(item.Photos.First().Content))
                });
            }
            SearchModel model = new SearchModel();
            model.Advertisements = itemsModelList;
            model.ListingsPageList = itemsModelList.ToPagedList(model.PageNumber, model.PageSize);

            return View("~/Views/Advertisement/Mine.cshtml", model);

        }


        public ActionResult Details(int id)
        {
            var advertisement = _advertisementService.Queryable().Include(x => x.Car.Model.Manufacturer).Include(x => x.User.Address).Include(x => x.Photos).Include(x => x.Car.Features).First(x => x.Id == id);
            var viewModel = new AdvertisementItemViewModel();
            viewModel.AdvertisementCurrent = advertisement;
            viewModel.User = advertisement.User;

            foreach (var feature in advertisement.Car.Features)
            {
                viewModel.Features.Add(feature);
            }

            var picturesModel = advertisement.Photos.Select(x =>
               new PictureModel()
               {
                   ID = x.Id,
                   Url = string.Format("data:{0};base64,{1}", x.Extension, Convert.ToBase64String(x.Content)),
                   AdvertisementId = advertisement.Id

               }).ToList();
            viewModel.Pictures = picturesModel;
            viewModel.UrlPicture = picturesModel.FirstOrDefault().Url;

            if (User.Identity.IsAuthenticated)
            {
                viewModel.UserId = User.Identity.GetUserId();
                viewModel.IsFollowing = _followingService.GetFollowing(viewModel.UserId, advertisement.UserId) != null;
            }

            return View(viewModel);
        }

        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new AdvertisementViewModel();
            viewModel.Manufacturers = _manufacturerService.Queryable().ToList();

            var allFeatures = _featureService.Queryable().ToList();
            var checkBoxListItems = new List<CheckBoxListItem>();

            foreach (var feature in allFeatures)
            {
                checkBoxListItems.Add(new CheckBoxListItem()
                {
                    Id = feature.Id,
                    Display = feature.Name,
                    IsChecked = false
                });
            }
            viewModel.Features = checkBoxListItems;
            return View("Form", viewModel);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create(AdvertisementViewModel viewModel)
        {
            if (!ModelState.IsValid || viewModel.PhotoFiles == null || viewModel.PhotoFiles.First() == null)
            {
                viewModel.Manufacturers = _manufacturerService.Queryable().ToList();
                if (viewModel.PhotoFiles == null || viewModel.PhotoFiles.First() == null)
                {
                    TempData[TempKeys.UserMessageAlertState] = "bg-danger";
                    TempData[TempKeys.UserMessage] = "Zdjęcia w ogłoszeniu są wymagane!";

                }
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

            var selectedFeatureIds = viewModel.Features.Where(x => x.IsChecked).Select(x => x.Id).ToList();
            foreach (var selectedFeatureId in selectedFeatureIds)
            {
                car.Features.Add(new Feature() { Id = selectedFeatureId });
            }
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
            return RedirectToAction("Details", "Advertisement", new { id = advertisement.Id });

        }
        public JsonResult FillModels(int manufacturer)
        {
            var models =
                _modelService.Queryable().Where(x => x.ManufacturerId == manufacturer).ToList();
            return Json(models, JsonRequestBehavior.AllowGet);
        }
        private async Task GetSearchResult(SearchModel model)
        {
            IEnumerable<Advertisement> items = null;

            // Category
            if (model.ManufacturerId != 0)
            {

                if (model.ModelId != 0)
                {
                    items = await _advertisementService.Query(x => x.Car.Model.Id == model.ModelId)
                        .Include(x => x.Car)
                        .Include(x => x.Car.Model)
                        .Include(x => x.Car.Model.Manufacturer)
                        .Include(x => x.Car.Features)
                        .Include(x => x.User)
                        .Include(x => x.Photos)
                        .Include(x => x.User.Address)
                        .SelectAsync();
                }
                else
                {
                    items = await _advertisementService.Query(x => x.Car.Model.ManufacturerId == model.ManufacturerId)
                  .Include(x => x.Car)
                  .Include(x => x.Car.Model)
                  .Include(x => x.Car.Model.Manufacturer)
                  .Include(x => x.User)
                                          .Include(x => x.Car.Features)

                                      .Include(x => x.Photos)
                  .Include(x => x.User.Address)
                  .SelectAsync();
                }
                model.Models = _modelService.Queryable().Where(x => x.ManufacturerId == model.ManufacturerId).ToList();


            }



            // Latest
            if (items == null)
            {
                items = await _advertisementService.Query().OrderBy(x => x.OrderByDescending(y => y.AddedDate))
                    .Include(x => x.Car)
                    .Include(x => x.Car.Model)
                    .Include(x => x.Car.Model.Manufacturer)
                    .Include(x => x.User)
                    .Include(x => x.Car.Features)

                    .Include(x => x.Photos)
                                        .Include(x => x.User.Address)
                    .SelectAsync();
            }


            // Location
            if (!string.IsNullOrEmpty(model.Location))
            {
                items = items.Where(x => !string.IsNullOrEmpty(x.User.Address.City) && string.Compare(x.User.Address.City.ToLower(), model.Location.ToLower(), CultureInfo.InvariantCulture, CompareOptions.IgnoreNonSpace) == 0);
            }


            if (model.PriceFrom.HasValue)
                items = items.Where(x => x.Car.Price >= model.PriceFrom.Value);

            if (model.PriceTo.HasValue)
                items = items.Where(x => x.Car.Price <= model.PriceTo.Value);

            if (model.SelectedFeatures != null)
            {
                var selectedAdvertisement = new List<Advertisement>();
                foreach (var advertisement in items.ToList())
                {
                    var contains = true;
                    foreach (var featureId in model.SelectedFeatures.ToList())
                    {
                        if (!advertisement.Car.Features.Select(x => x.Id).Contains(featureId))
                        {
                            contains = false;
                            break;
                        }
                    }
                    if (contains)
                    {
                        selectedAdvertisement.Add(advertisement);
                    }
                }
                items = selectedAdvertisement;
            }

            var itemsModelList = new List<ListingItemModel>();
            foreach (var item in items.Where(x => x.IsActive).OrderByDescending(x => x.AddedDate))
            {
                itemsModelList.Add(new ListingItemModel()
                {
                    ListingCurrent = item,
                    UrlPicture = item.Photos.Count == 0 ? "" : string.Format("data:{0};base64,{1}", item.Photos.First().Extension, Convert.ToBase64String(item.Photos.First().Content))
                });
            }

            model.Manufacturers = _manufacturerService.Queryable().ToList();

            model.Advertisements = itemsModelList;
            model.ListingsPageList = itemsModelList.ToPagedList(model.PageNumber, model.PageSize);
            model.Features = _featureService.Queryable().ToList();



        }

        public async Task<ActionResult> Profile(string id)
        {

            var items = await _advertisementService.Query(x => x.UserId == id)
                        .Include(x => x.Car)
                        .Include(x => x.Car.Model)
                        .Include(x => x.Car.Model.Manufacturer)
                        .Include(x => x.User)
                        .Include(x => x.Photos)
                        .Include(x => x.User.Address)
                        .SelectAsync();
            var itemsModelList = new List<ListingItemModel>();
            foreach (var item in items.Where(x => x.IsActive).OrderByDescending(x => x.AddedDate))
            {
                itemsModelList.Add(new ListingItemModel()
                {
                    ListingCurrent = item,
                    UrlPicture = item.Photos.Count == 0 ? "" : string.Format("data:{0};base64,{1}", item.Photos.First().Extension, Convert.ToBase64String(item.Photos.First().Content))
                });
            }
            SearchModel model = new SearchModel();
            model.Advertisements = itemsModelList;
            model.ListingsPageList = itemsModelList.ToPagedList(model.PageNumber, model.PageSize);

            return View("~/Views/Advertisement/Profile.cshtml", model);
        }
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var advertisement = await _advertisementService.FindAsync(id);
            advertisement.IsActive = false;
            advertisement.ObjectState = ObjectState.Modified;

            await _unitOfWorkAsync.SaveChangesAsync();


            return RedirectToAction("Index");
        }
    }
}