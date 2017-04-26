using AutoMoto.Models;
using AutoMoto.Service;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace AutoMoto.Web.Controllers.API
{
    public class NotificationsController : ApiController
    {
        private readonly SqlDbService _sqlDbService;
        public NotificationsController(SqlDbService sqlDbService)
        {
            _sqlDbService = sqlDbService;
        }
        public List<Notification> GetNewNotifications()
        {
            var userId = User.Identity.GetUserId();
            List<Notification> notifications = _sqlDbService.GetNewNotificationsFor(userId).ToList();
            return notifications;
        }

        [HttpPost]
        public async Task<IHttpActionResult> MarkAsRead()
        {
            var userId = User.Identity.GetUserId();
            _sqlDbService.MarkAsRead(userId);
            return Ok();
        }
    }
}
