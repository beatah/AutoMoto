using AutoMoto.Model.Models;
using System.Web.Mvc;

namespace AutoMoto.Web.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : Controller
    {



        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dictionary()
        {
            return View();
        }



    }
}