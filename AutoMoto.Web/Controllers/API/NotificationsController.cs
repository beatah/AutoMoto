using AutoMoto.Contracts.Interfaces;
using AutoMoto.Models;
using Microsoft.AspNet.Identity;
using Repository.Pattern.UnitOfWork;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace AutoMoto.Web.Controllers.API
{
    public class NotificationsController : ApiController
    {
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly IUserNotificationService _userNotificationService;

        public NotificationsController(IUnitOfWorkAsync unitOfWork, IUserNotificationService userNotificationService)
        {
            _unitOfWork = unitOfWork;
            _userNotificationService = userNotificationService;
        }
        public List<Notification> GetNewNotifications()
        {
            var userId = User.Identity.GetUserId();
            List<Notification> notifications = _userNotificationService.GetNewNotificationsFor(userId).Select(x => x.Notification).OfType<FollowingNotification>().Include(x => x.Advertisement).Include(x => x.Advertisement.User).Cast<Notification>().ToList();
            return notifications;
        }

        [HttpPost]
        public async Task<IHttpActionResult> MarkAsRead()
        {
            var userId = User.Identity.GetUserId();
            var notifications = _userNotificationService.Queryable().Where(x => x.UserId == userId).ToList();

            foreach (var userNotification in notifications)
            {
                userNotification.IsRead = true;
                _userNotificationService.Update(userNotification);
            }

            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }
    }
}
