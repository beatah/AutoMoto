using AutoMoto.Contracts.Interfaces;
using AutoMoto.Contracts.ViewModels;
using AutoMoto.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutoMoto.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IManufacturerService _manufacturerService;
        private readonly IUserNotificationService _userNotificationService;
        private readonly IModelService _modelService;
        private readonly IAdvertisementService _advertisementService;

        public HomeController(IManufacturerService manufacturerService, IModelService modelService, IAdvertisementService advertisementService, IUserNotificationService userNotificationService)
        {
            _manufacturerService = manufacturerService;
            _modelService = modelService;
            _advertisementService = advertisementService;
            _userNotificationService = userNotificationService;
        }

        public async Task<ActionResult> Messages()
        {
            var userId = User.Identity.GetUserId();
            var messages = _userNotificationService.GetNewNotificationsFor(userId).Select(x => x.Notification).OfType<MessageNotification>().Include(x => x.Message).Include(x => x.Message.FromUser).Include(x => x.Message.Advertisement).ToList();
            return PartialView("_Message", messages);
        }
        public async Task<ActionResult> Index(int? page)
        {
            var model = new SearchModel();
            await GetSearchResult(model);

            return View(model);


            //int pageSize = 2;
            //int pageIndex = 1;
            //pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            //var advertisements =
            //    _advertisementService.Queryable().Include(x => x.Car.Model.Manufacturer).Include(x => x.Photos).OrderByDescending(x => x.AddedDate).ToPagedList(pageIndex, pageSize); ;

            //return View(advertisements);
        }
        private async Task GetSearchResult(SearchModel model)
        {
            IEnumerable<Advertisement> items = null;

            // Category
            if (model.ManufacturerId != 0)
            {
                items = await _advertisementService.Query(x => x.Car.Model.ManufacturerId == model.ManufacturerId)
                    .Include(x => x.Car)
                    .Include(x => x.Car.Model)
                    .Include(x => x.Car.Model.Manufacturer)
                    .Include(x => x.User)
                    .Include(x => x.Photos)

                    .Include(x => x.User.Address)
                    .SelectAsync();

                // Set listing types
                // model.ListingTypes = CacheHelper.ListingTypes.Where(x => x.CategoryListingTypes.Any(y => y.CategoryID == model.CategoryID)).ToList();
            }
            else
            {
                // model.ListingTypes = CacheHelper.ListingTypes;
            }

            // Set default Listing Type if it's not set or listing type is not set
            //if (model.ListingTypes.Count > 0 &&
            //    (model.ListingTypeID == null || !model.ListingTypes.Any(x => model.ListingTypeID.Contains(x.ID))))
            //{
            //    model.ListingTypeID = new List<int>();
            //    model.ListingTypeID.Add(model.ListingTypes.FirstOrDefault().ID);
            //}




            // Latest
            if (items == null)
            {
                items = await _advertisementService.Query().OrderBy(x => x.OrderByDescending(y => y.AddedDate))
                    .Include(x => x.Car)
                    .Include(x => x.Car.Model)
                    .Include(x => x.Car.Model.Manufacturer)
                    .Include(x => x.User)
                    .Include(x => x.Photos)
                                        .Include(x => x.User.Address)
                    .SelectAsync();
            }

            // Filter items by Listing Type
            //   items = items.Where(x => model.ListingTypeID.Contains(x.ListingTypeID));

            // Location
            if (!string.IsNullOrEmpty(model.Location))
            {
                items = items.Where(x => !string.IsNullOrEmpty(x.User.Address.City) && x.User.Address.City.IndexOf(model.Location, StringComparison.OrdinalIgnoreCase) != -1);
            }

            // Picture
            //if (model.PhotoOnly)
            //    items = items.Where(x => x.ListingPictures.Count > 0);

            /// Price
            if (model.PriceFrom.HasValue)
                items = items.Where(x => x.Car.Price >= model.PriceFrom.Value);

            if (model.PriceTo.HasValue)
                items = items.Where(x => x.Car.Price <= model.PriceTo.Value);

            // Show active and enabled only
            var itemsModelList = new List<ListingItemModel>();
            foreach (var item in items.Where(x => x.IsActive).OrderByDescending(x => x.AddedDate))
            {
                itemsModelList.Add(new ListingItemModel()
                {
                    ListingCurrent = item,
                    UrlPicture = item.Photos.Count == 0 ? "" : string.Format("data:{0};base64,{1}", item.Photos.First().Extension, Convert.ToBase64String(item.Photos.First().Content))
                });
            }
            // var breadCrumb = GetParents(model.CategoryID).Reverse().ToList();

            //  model.BreadCrumb = breadCrumb;
            model.Manufacturers = _manufacturerService.Queryable().ToList();

            model.Advertisements = itemsModelList;
            model.ListingsPageList = itemsModelList.ToPagedList(model.PageNumber, model.PageSize);
            //  model.Grid = new ListingModelGrid(model.ListingsPageList.AsQueryable());
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}