using AutoMoto.Contracts.Dtos;
using AutoMoto.Contracts.Interfaces;
using AutoMoto.Models;
using Microsoft.AspNet.Identity;
using Repository.Pattern.UnitOfWork;
using System.Threading.Tasks;
using System.Web.Http;

namespace AutoMoto.Web.Controllers.API
{
    public class FollowingController : ApiController
    {
        private readonly IFollowingService _followingService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public FollowingController(IFollowingService followingService, IUnitOfWorkAsync unitOfWorkAsync)
        {
            _followingService = followingService;
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Follow(FollowingDto dto)
        {
            var userId = User.Identity.GetUserId();

            var following = _followingService.GetFollowing(userId, dto.FolloweeId);
            if (following != null)
                return BadRequest("Obserwowanie już istnieje");

            following = new Following
            {
                FollowerId = userId,
                FolloweeId = dto.FolloweeId
            };
            _followingService.Insert(following);
            await _unitOfWorkAsync.SaveChangesAsync();

            return Ok();
        }
        [HttpDelete]
        public async Task<IHttpActionResult> Unfollow(string id)
        {
            var userId = User.Identity.GetUserId();

            var following = _followingService.GetFollowing(userId, id);

            if (following == null)
                return NotFound();

            _followingService.Delete(following);
            await _unitOfWorkAsync.SaveChangesAsync();

            return Ok(id);
        }
    }
}
