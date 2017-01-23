using AutoMoto.Model.Models;
using AutoMoto.Models;
using System.Collections.Generic;

namespace AutoMoto.Contracts.ViewModels
{
    public class AdvertisementItemViewModel
    {

        public Advertisement AdvertisementCurrent { get; set; }

        public string UrlPicture { get; set; }

        public List<PictureModel> Pictures { get; set; }
        public List<Feature> Features { get; set; }


        public AspNetUser User { get; set; }
        public string UserId { get; set; }
        public bool IsFollowing { get; set; }

        public AdvertisementItemViewModel()
        {
            Features = new List<Feature>();
        }
    }

    public class PictureModel
    {
        public int ID { get; set; }

        public int AdvertisementId { get; set; }

        public string Url { get; set; }

    }
}
