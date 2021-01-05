using System.Threading.Tasks;
using Dropship.Website.Backend.Database.Entities;
using Dropship.Website.Backend.Models.Responses;
using Dropship.Website.Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Dropship.Website.Backend.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AuthService _authService;

        public UsersController(AuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [EnableCors("anyorigin")]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var userEntity = await _authService.GetUserById(userId);
            if (userEntity == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Plugin with that Id does not exist."
                });
            }

            userEntity.Password = null;
            userEntity.Mods.ForEach(s => s.Creator = null);
            userEntity.Plugins.ForEach(s => s.Creator = null);
            userEntity.Servers.ForEach(s => s.Owner = null);

            return Ok(new GenericResponse<UserEntity>
            {
                Success = true,
                Data = userEntity
            });
        }
    }
}