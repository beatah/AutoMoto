using AutoMoto.Models;

namespace AutoMoto.Contracts.Dtos
{
    public class FollowingNotificationDto : NotificationDto
    {
        public Advertisement Advertisement { get; set; }
    }
}
