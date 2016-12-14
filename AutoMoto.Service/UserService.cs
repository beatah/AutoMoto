using AutoMoto.Contracts.Interfaces;
using AutoMoto.Model.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;

namespace AutoMoto.Service
{
    public class UserService : Service<AspNetUser>, IUserService
    {
        public UserService(IRepositoryAsync<AspNetUser> repository) : base(repository)
        {
        }
    }
}