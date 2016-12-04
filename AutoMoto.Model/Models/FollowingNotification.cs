using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoMoto.Models
{
    [Table("FollowingNotifications")]
    //obserwowany uzytkownik dodal nowe ogloszenie
    public class FollowingNotification : Notification
    {
        [Key]
        public int AdvertisementId { get; set; }
        public Advertisement Advertisement { get; set; }

    }
}