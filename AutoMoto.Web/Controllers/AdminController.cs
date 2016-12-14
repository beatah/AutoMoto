using AutoMoto.Contracts.Interfaces;
using AutoMoto.Contracts.ViewModels;
using AutoMoto.Model.Models;
using System.Linq;
using System.Web.Mvc;

namespace AutoMoto.Web.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : Controller
    {

        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        private readonly IAdvertisementService _advertisementService;

        public AdminController(IUserService userService, IMessageService messageService, IAdvertisementService advertisementService)
        {
            _userService = userService;
            _messageService = messageService;
            _advertisementService = advertisementService;
        }

        public ActionResult Index()
        {
            var summary = new SummaryViewModel();
            summary.AdvCount = _advertisementService.Queryable().Count();
            summary.UserCount = _userService.Queryable().Count();
            summary.MessagesCount = _messageService.Queryable().Count();
            return View(summary);
        }
        public ActionResult Dictionary()
        {
            return View();
        }



    }
}