using AutoMoto.Model.Models;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoMoto.Models
{
    public class UserNotification : Entity
    {

        [Key]
        [Column(Order = 1)]
        public int NotificationId { get; set; }
        public Notification Notification { get; set; }

        [Key]
        [Column(Order = 2)]
        public string UserId { get; set; }
        public AspNetUser User { get; set; }
        public bool IsRead { get; set; }

    }
}