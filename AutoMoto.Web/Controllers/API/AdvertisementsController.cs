using AutoMoto.Contracts.Interfaces;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace AutoMoto.Web.Controllers.API
{
    public class AdvertisementsController : ApiController
    {
        private readonly IModelService _modelService;

        public AdvertisementsController(IModelService modelService)
        {
            _modelService = modelService;
        }


        public JsonResult FillModels(int manufacturer)
        {
            var models =
                _modelService.Queryable().Where(x => x.ManufacturerId == manufacturer).ToList();
            return Json(models, JsonRequestBehavior.AllowGet);
        }
    }
}
