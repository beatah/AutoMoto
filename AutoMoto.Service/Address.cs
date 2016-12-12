using AutoMoto.Contracts.Interfaces;
using AutoMoto.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;

namespace AutoMoto.Service
{
    public class AddressService : Service<Address>, IAddressService
    {
        public AddressService(IRepositoryAsync<Address> repository) : base(repository)
        {
        }
    }
}
