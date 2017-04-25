using AutoMoto.Contracts.Interfaces;
using AutoMoto.Contracts.ViewModels;
using AutoMoto.Service;
using Repository.Pattern.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutoMoto.Web.Controllers
{
    public class ModelController : Controller
    {
        private readonly SqlDbService _sqlDbService;


        public ModelController(IModelService modelService, IUnitOfWorkAsync unitOfWorkAsync, IManufacturerService manufacturerService, SqlDbService sqlDbService)
        {
            _sqlDbService = sqlDbService;
        }
        public ActionResult Index()
        {
            var list =
                _sqlDbService.GetAllModels().ToList();
            var group = list
                    .GroupBy(m => m.Manufacturer);
            var models = group
                    .Select(m => new { i = m.Key.Name, j = m.Key.Models.OrderBy(x => x.Name).ToList() })
                    .OrderBy(x => x.i)
                    .ToDictionary(x => x.i, x => x.j);
            var newManufacturers = _sqlDbService.GetAllManufactures().Where(x => x.Models.Count == 0).ToList();
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
            _sqlDbService.InsertModel(viewModel.Name, viewModel.Manufacturer);

            return RedirectToAction("Index");
        }
    }
}