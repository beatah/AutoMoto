using AutoMoto.Contracts.Interfaces;
using Repository.Pattern.UnitOfWork;
using System.Linq;
using System.Web.Mvc;

namespace AutoMoto.Web.Controllers
{
    public class ModelController : Controller
    {
        private readonly IModelService _modelService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public ModelController(IModelService modelService, IUnitOfWorkAsync unitOfWorkAsync)
        {
            _modelService = modelService;
            _unitOfWorkAsync = unitOfWorkAsync;
        }
        public ActionResult Index()
        {
            var models =
                _modelService.Queryable()
                    .GroupBy(m => m.Manufacturer)
                    .Select(m => new { i = m.Key.Name, j = m.Key.Models.ToList() })
                    .ToDictionary(x => x.i, x => x.j);

            return View("Index", models);
        }
    }
}