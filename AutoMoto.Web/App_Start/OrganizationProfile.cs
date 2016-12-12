using AutoMapper;
using AutoMoto.Contracts.Dtos;
using AutoMoto.Contracts.ViewModels;
using AutoMoto.Models;

namespace AutoMoto.Web.App_Start
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<Manufacturer, ManufacturerViewModel>();
            CreateMap<Advertisement, AdvertisementDetailsViewModel>();
            CreateMap<Advertisement, AdvertisementViewModel>();
            CreateMap<Car, AdvertisementViewModel>();
            CreateMap<Notification, NotificationDto>();
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Notification, NotificationDto>()
                    .Include<FollowingNotification, FollowingNotificationDto>();
                cfg.CreateMap<FollowingNotification, FollowingNotificationDto>();

            });

        }
    }
}