using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Dropship.Website.Backend.Database.Entities;
using Dropship.Website.Backend.Models.Requests.ModBuilds;
using Dropship.Website.Backend.Models.Responses;
using Dropship.Website.Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Dropship.Website.Backend.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ModBuildsController : ControllerBase
    {
        private readonly ModBuildsService _modBuildsService;
        private readonly ModsService _modsService;

        public ModBuildsController(ModBuildsService modBuildsService, ModsService modsService)
        {
            _modBuildsService = modBuildsService;
            _modsService = modsService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateModBuildAsync(CreateModBuildRequest request)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            if (!await _modsService.UserIdOwnsModIdAsync(userId, request.ModId))
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Bad ModId for creating a build"
                });
            }
            var modBuildEntity = await _modBuildsService.CreateModBuildAsync(request);
            if (modBuildEntity == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Unable to create build."
                });
            }
            return Ok(new GenericResponse<ModBuildEntity>
            {
                Success = true,
                Message = "Created mod build.",
                Data = modBuildEntity
            });
        }

        [AllowAnonymous]
        [EnableCors("anyorigin")]
        [HttpGet("{modId}/{versionCode}")]
        public async Task<IActionResult> GetModBuildAsync(int modId, int versionCode)
        {
            var modBuildEntity = await _modBuildsService.GetModBuildAsync(modId, versionCode);
            if (modBuildEntity == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Mod build does not exist."
                });
            }
            return Ok(new GenericResponse<ModBuildEntity>
            {
                Success = true,
                Data = modBuildEntity
            });
        }
        
        [AllowAnonymous]
        [EnableCors("anyorigin")]
        [HttpGet("{modId}/page/{pageNumber}")]
        public async Task<IActionResult> GetModBuildListPaginatedAsync(int modId, int pageNumber)
        {
            var modBuildsPaginated = await _modBuildsService.ModBuildsPaginatedAsync(modId, pageNumber);
            if (modBuildsPaginated == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Internal server error."
                });
            }

            return Ok(new GenericResponse<List<ModBuildEntity>>
            {
                Success = true,
                Data = modBuildsPaginated
            });
        }
        
        [Authorize]
        [HttpDelete("{modId}/{versionCode}")]
        public async Task<IActionResult> DeleteModBuildAsync(int modId, int versionCode)
        {
            var modBuild = await _modBuildsService.GetModBuildAsync(modId, versionCode);
            if (modBuild == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Mod build for that modId and/or version do not exist."
                });
            }
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            if (!await _modsService.UserIdOwnsModIdAsync(userId, modId))
            {
                return Unauthorized(new GenericResponse
                {
                    Success = false,
                    Message = "Unauthorized to delete mod build."
                });
            }

            await _modBuildsService.DeleteModBuildAsync(modBuild);
            return Ok(new GenericResponse
            {
                Success = true,
                Message = "Deleted mod build."
            });
        }
    }
}