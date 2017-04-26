using System;

namespace AutoMoto.Model.Database_Objects.Results
{
    public class NewFollowingNotificationResult
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int AdvertisementId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
    }
}
