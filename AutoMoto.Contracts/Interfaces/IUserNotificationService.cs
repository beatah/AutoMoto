using AutoMoto.Models;
using Service.Pattern;
using System.Linq;

namespace AutoMoto.Contracts.Interfaces
{
    public interface IUserNotificationService : IService<UserNotification>
    {
        IQueryable<UserNotification> GetNewNotificationsFor(string userId);
    }
}