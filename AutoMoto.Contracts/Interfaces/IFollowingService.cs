using AutoMoto.Models;
using Service.Pattern;
using System.Linq;

namespace AutoMoto.Contracts.Interfaces
{
    public interface IFollowingService : IService<Following>
    {
        Following GetFollowing(string followerId, string followeeId);
        IQueryable<Following> GetFollowingByFollowee(string followeeId);
    }
}