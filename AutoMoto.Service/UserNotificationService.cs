using AutoMoto.Contracts.Interfaces;
using AutoMoto.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System.Linq;

namespace AutoMoto.Service
{
    public class UserNotificationService : Service<UserNotification>, IUserNotificationService
    {
        public UserNotificationService(IRepositoryAsync<UserNotification> repository) : base(repository)
        {
        }
        public IQueryable<UserNotification> GetNewNotificationsFor(string userId)
        {
            return Queryable().Where(x => x.IsRead == false && x.UserId == userId);
        }


    }
}
