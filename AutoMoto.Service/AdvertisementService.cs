using AutoMoto.Contracts.Interfaces;
using AutoMoto.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;

namespace AutoMoto.Service
{
    public class AdvertisementService : Service<Advertisement>, IAdvertisementService
    {
        public AdvertisementService(IRepositoryAsync<Advertisement> repository) : base(repository)
        {
        }

    }
}
