using System.ComponentModel.DataAnnotations.Schema;

namespace AutoMoto.Models
{
    [Table("MessageNotifications")]
    public class MessageNotification : Notification
    {
        public int MessageId { get; set; }
        public Message Message { get; set; }

    }
}