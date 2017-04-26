using AutoMoto.Contracts.Dtos;
using AutoMoto.Service;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Http;

namespace AutoMoto.Web.Controllers.API
{
    public class FollowingController : ApiController
    {
        private readonly SqlDbService _sqlDbService;

        public FollowingController(SqlDbService sqlDbService)
        {
            _sqlDbService = sqlDbService;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Follow(FollowingDto dto)
        {
            var userId = User.Identity.GetUserId();
            var isExists = _sqlDbService.IsFollowingExists(userId, dto.FolloweeId);

            if (isExists)
                return BadRequest("Obserwowanie już istnieje");

            _sqlDbService.InsertFollowing(userId, dto.FolloweeId);

            return Ok();
        }
        [HttpDelete]
        public async Task<IHttpActionResult> Unfollow(string id)
        {
            var userId = User.Identity.GetUserId();

            _sqlDbService.DeleteFollowing(userId, id);

            return Ok(id);
        }
    }
}
