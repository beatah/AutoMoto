using AutoMapper;
using AutoMoto.Contracts.Interfaces;
using AutoMoto.Contracts.ViewModels;
using AutoMoto.Model.Models;
using AutoMoto.Models;
using AutoMoto.Service;
using Repository.Pattern.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AutoMoto.Web.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class ManufacturerController : Controller
    {
        private readonly SqlDbService _sqlDbService;

        public ManufacturerController(IManufacturerService manufacturerService, IUnitOfWorkAsync unitOfWorkAsync, SqlDbService sqlDbService)
        {
            _sqlDbService = sqlDbService;
        }
        // GET: Manufacturer
        public ActionResult Index()
        {
            var manufacturers = _sqlDbService.GetAllManufactures().ToList();

            return View(Mapper.Map<IEnumerable<Manufacturer>, IEnumerable<ManufacturerViewModel>>(manufacturers));
        }
        public ActionResult Create()
        {
            return View("Create");
        }
        [HttpPost]
        public ActionResult Create(ManufacturerFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", viewModel);
            }

            _sqlDbService.InsertManufacturer(viewModel.Name);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var manufacturer = _sqlDbService.GetManufacturerById(id);
            var viewModel = Mapper.Map<Manufacturer, ManufacturerViewModel>(manufacturer);

            return View("Edit", viewModel);
        }
        [HttpPost]
        public ActionResult Edit(ManufacturerViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", viewModel);
            }

            var status = viewModel.IsActive ? 1 : 0;
            _sqlDbService.ChangeManufacturerStatus(viewModel.Id, status);

            return RedirectToAction("Index");
        }
    }
}