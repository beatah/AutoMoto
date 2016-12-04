using AutoMapper;
using AutoMoto.Contracts.Interfaces;
using AutoMoto.Contracts.ViewModels;
using AutoMoto.Model.Models;
using AutoMoto.Models;
using Repository.Pattern.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutoMoto.Web.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class ManufacturerController : Controller
    {
        private readonly IManufacturerService _manufacturerService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public ManufacturerController(IManufacturerService manufacturerService, IUnitOfWorkAsync unitOfWorkAsync)
        {
            _manufacturerService = manufacturerService;
            _unitOfWorkAsync = unitOfWorkAsync;
        }
        // GET: Manufacturer
        public ActionResult Index()
        {
            var manufacturers = _manufacturerService.Queryable().ToList();

            return View(Mapper.Map<IEnumerable<Manufacturer>, IEnumerable<ManufacturerViewModel>>(manufacturers));
        }
        public ActionResult Create()
        {
            return View("Create");
        }
        [HttpPost]
        public async Task<ActionResult> Create(ManufacturerFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", viewModel);
            }

            var entity = new Manufacturer()
            {
                Name = viewModel.Name
            };
            _manufacturerService.Insert(entity);

            await _unitOfWorkAsync.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int id)
        {
            var manufacturer = await _manufacturerService.FindAsync(id);
            var viewModel = Mapper.Map<Manufacturer, ManufacturerViewModel>(manufacturer);

            return View("Edit", viewModel);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(ManufacturerViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", viewModel);
            }

            var manufacturer = await _manufacturerService.FindAsync(viewModel.Id);
            if (manufacturer.IsActive != viewModel.IsActive)
            {
                manufacturer.IsActive = viewModel.IsActive;
                _manufacturerService.Update(manufacturer);

                await _unitOfWorkAsync.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}