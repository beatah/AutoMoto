using AutoMapper;
using AutoMoto.Contracts.ViewModels;
using AutoMoto.Models;

namespace AutoMoto.Web.App_Start
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<Manufacturer, ManufacturerViewModel>();
        }
    }
}