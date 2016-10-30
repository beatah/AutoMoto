using AutoMoto.Contracts.Interfaces;
using AutoMoto.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;

namespace AutoMoto.Service
{
    public class ManufacturerService : Service<Manufacturer>, IManufacturerService
    {
        public ManufacturerService(IRepositoryAsync<Manufacturer> repository) : base(repository)
        {
        }
    }
}
