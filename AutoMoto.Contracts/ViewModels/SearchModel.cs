using AutoMoto.Model.Models;
using AutoMoto.Models;
using Microsoft.Build.Framework.XamlTypes;
using PagedList;
using System.Collections.Generic;

namespace AutoMoto.Contracts.ViewModels
{
    public class SearchModel : SortViewModel
    {
        public int ManufacturerId { get; set; }
        public int ModelId { get; set; }



        public string SearchText { get; set; }
        public string Location { get; set; }



        public decimal? PriceFrom { get; set; }

        public decimal? PriceTo { get; set; }

        public List<ListingItemModel> Advertisements { get; set; }

        public IPagedList<ListingItemModel> ListingsPageList { get; set; }

        public List<Category> Categories { get; set; }


        public List<Manufacturer> Manufacturers { get; set; }
        public List<Models.Model> Models { get; set; }


    }

    public class SortViewModel
    {
        public SortViewModel()
        {
            PageSize = 6;
            PageNumber = 1;
        }

        public int SortBy { get; set; }

        public int SortCriteriaId { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

    }

    public class ListingItemModel
    {
        public List<Advertisement> ListingsOther { get; set; }

        public Advertisement ListingCurrent { get; set; }

        public string UrlPicture { get; set; }

        public List<PictureModel> Pictures { get; set; }


        public AspNetUser User { get; set; }

    }
}
