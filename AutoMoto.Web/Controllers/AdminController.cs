using AutoMoto.Contracts.Interfaces;
using AutoMoto.Contracts.ViewModels;
using AutoMoto.Model.Models;
using AutoMoto.Models;
using Repository.Pattern.UnitOfWork;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutoMoto.Web.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : Controller
    {
        private readonly IManufacturerService _manufacturerService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public AdminController(IManufacturerService manufacturerService, IUnitOfWorkAsync unitOfWorkAsync)
        {
            _manufacturerService = manufacturerService;
            _unitOfWorkAsync = unitOfWorkAsync;
        }
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult CreateManufacturer()
        {
            var viewModel = new ManufacturerViewModel { Action = "CreateManufacturer" };
            return View("ManufacturerForm", viewModel);
        }
        [HttpPost]
        public async Task<ActionResult> CreateManufacturer(ManufacturerViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ManufacturerForm", viewModel);
            }

            var entity = new Manufacturer()
            {
                Name = viewModel.Name
            };
            _manufacturerService.Insert(entity);

            await _unitOfWorkAsync.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}