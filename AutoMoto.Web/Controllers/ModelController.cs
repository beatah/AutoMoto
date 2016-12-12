using AutoMoto.Contracts.Interfaces;
using AutoMoto.Contracts.ViewModels;
using Repository.Pattern.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutoMoto.Web.Controllers
{
    public class ModelController : Controller
    {
        private readonly IModelService _modelService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public ModelController(IModelService modelService, IUnitOfWorkAsync unitOfWorkAsync, IManufacturerService manufacturerService)
        {
            _modelService = modelService;
            _unitOfWorkAsync = unitOfWorkAsync;
            _manufacturerService = manufacturerService;
        }
        public ActionResult Index()
        {
            var models =
                _modelService.Queryable()
                    .GroupBy(m => m.Manufacturer)
                    .Select(m => new { i = m.Key.Name, j = m.Key.Models.OrderBy(x => x.Name).ToList() })
                    .OrderBy(x => x.i)
                    .ToDictionary(x => x.i, x => x.j);
            var newManufacturers = _manufacturerService.Queryable().Where(x => x.Models.Count == 0).ToList();
            if (newManufacturers.Any())
            {
                foreach (var manufacturer in newManufacturers)
                {
                    models.Add(manufacturer.Name, new List<AutoMoto.Models.Model>());

                }
            }
            return View("Index", models);
        }

        public ActionResult Create(string manufacturer)
        {
            var viewModel = new ModelFormViewModel() { Manufacturer = manufacturer };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<ActionResult> Create(ModelFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", viewModel);
            }
            var manufacturer = await _manufacturerService.Query(x => x.Name == viewModel.Manufacturer).SelectAsync();
            var entity = new AutoMoto.Models.Model()
            {
                Name = viewModel.Name,
                ManufacturerId = manufacturer.First().Id
            };
            _modelService.Insert(entity);

            await _unitOfWorkAsync.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}