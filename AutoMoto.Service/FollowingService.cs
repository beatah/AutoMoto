using AutoMoto.Contracts.Interfaces;
using AutoMoto.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System.Linq;

namespace AutoMoto.Service
{
    public class FollowingService : Service<Following>, IFollowingService
    {
        public FollowingService(IRepositoryAsync<Following> repository) : base(repository)
        {
        }
        public Following GetFollowing(string followerId, string followeeId)
        {
            return Queryable().SingleOrDefault(f => f.FolloweeId == followeeId && f.FollowerId == followerId);
        }

        public IQueryable<Following> GetFollowingByFollowee(string followeeId)
        {
            return Queryable().Where(x => x.FolloweeId == followeeId);
        }
    }
}
