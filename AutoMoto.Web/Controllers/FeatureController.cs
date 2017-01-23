using AutoMoto.Contracts.Interfaces;
using AutoMoto.Contracts.ViewModels;
using AutoMoto.Model.Models;
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
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public FeatureController(IFeatureService featureService, IUnitOfWorkAsync unitOfWorkAsync)
        {
            _featureService = featureService;
            _unitOfWorkAsync = unitOfWorkAsync;
        }
        // GET: Manufacturer
        public ActionResult Index()
        {
            var features = _featureService.Queryable().ToList();

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

            var entity = new Feature()
            {
                Name = viewModel.Name
            };
            _featureService.Insert(entity);

            await _unitOfWorkAsync.SaveChangesAsync();

            return RedirectToAction("Index");
        }


    }
}