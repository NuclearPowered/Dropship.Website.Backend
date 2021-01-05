using System.Security.Claims;
using System.Threading.Tasks;
using Dropship.Website.Backend.Models.Requests.ServerJoin;
using Dropship.Website.Backend.Models.Responses;
using Dropship.Website.Backend.Models.Responses.ServerJoin;
using Dropship.Website.Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dropship.Website.Backend.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ServerJoinController : ControllerBase
    {
        private readonly ServerJoinService _srv;

        public ServerJoinController(ServerJoinService service)
        {
            _srv = service;
        }

        [Authorize]
        [HttpPost("willjoin")]
        public async Task<IActionResult> ClientWillJoinAsync([FromBody] ClientWillJoinRequest request)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            await _srv.ClientWillJoinAsync(request.ServerNonce, userId);
            return Ok(new GenericResponse
            {
                Success = true
            });
        }

        [AllowAnonymous]
        [HttpPost("checkjoin")]
        public async Task<IActionResult> CheckClientJoinAsync([FromBody] CheckClientJoinRequest request)
        {
            var userEntity = await _srv.CheckClientJoinAsync(request.UserId, request.ServerNonce);
            if (userEntity == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false
                });
            }
            return Ok(new GenericResponse<UserOverviewResponse>
            {
                Success = true,
                Data = new UserOverviewResponse
                {
                    Id = userEntity.Id,
                    Username = userEntity.Username
                }
            });
        }
    }
}