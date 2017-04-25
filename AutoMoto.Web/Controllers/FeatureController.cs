using AutoMoto.Contracts.Interfaces;
using AutoMoto.Contracts.ViewModels;
using AutoMoto.Model.Models;
using AutoMoto.Service;
using Repository.Pattern.UnitOfWork;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutoMoto.Web.Controllers
{

    [Authorize(Roles = Roles.Admin)]

    public class FeatureController : Controller
    {
        private readonly IFeatureService _featureService;
        private readonly SqlDbService _sqlDbService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public FeatureController(IFeatureService featureService, IUnitOfWorkAsync unitOfWorkAsync, SqlDbService sqlDbService)
        {
            _featureService = featureService;
            _unitOfWorkAsync = unitOfWorkAsync;
            _sqlDbService = sqlDbService;
        }
        // GET: Manufacturer
        public ActionResult Index()
        {
            var features = _sqlDbService.GetAllFeatures().ToList();

            return View(features);
        }
        public ActionResult Create()
        {
            return View("Create");
        }
        [HttpPost]
        public async Task<ActionResult> Create(FeatureFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", viewModel);
            }

            _sqlDbService.InsertFeature(viewModel.Name);

            return RedirectToAction("Index");
        }


    }
}