using AutoMoto.Model.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoMoto.Models
{
    [Table("FollowingNotifications")]
    //obserwowany uzytkownik dodal nowe ogloszenie
    public class FollowingNotification : Notification
    {
        [Key]
        [Column(Order = 1)]
        public string UserId { get; set; }
        public AspNetUser User { get; set; }
        [Key]
        [Column(Order = 2)]
        public int AdvertisementId { get; set; }
        public Advertisement Advertisement { get; set; }

    }
}