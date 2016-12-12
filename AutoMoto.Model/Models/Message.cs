using AutoMoto.Model.Models;
using Repository.Pattern.Ef6;

namespace AutoMoto.Models
{
    public class Message : Entity
    {
        public int Id { get; set; }
        public int Thread { get; set; }
        public AspNetUser FromUser { get; set; }
        public string FromUserId { get; set; }
        public AspNetUser ToUser { get; set; }
        public string ToUserId { get; set; }

        public string Content { get; set; }
        public Advertisement Advertisement { get; set; }
        public int AdvertisementId { get; set; }
    }
}