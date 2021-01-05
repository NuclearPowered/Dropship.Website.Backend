using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Dropship.Website.Backend.Database.Entities;
using Dropship.Website.Backend.Models.Requests.PluginBuilds;
using Dropship.Website.Backend.Models.Responses;
using Dropship.Website.Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Dropship.Website.Backend.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class PluginBuildsController : ControllerBase
    {
        private readonly PluginBuildService _pluginBuildService;
        private readonly PluginService _pluginService;

        public PluginBuildsController(PluginBuildService pluginBuildService, PluginService pluginService)
        {
            _pluginBuildService = pluginBuildService;
            _pluginService = pluginService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreatePluginBuildAsync(CreatePluginBuildRequest request)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            if (!await _pluginService.UserIdOwnsPluginIdAsync(userId, request.PluginId))
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Bad PluginId for creating a build"
                });
            }
            var pluginBuildEntity = await _pluginBuildService.CreatePluginBuildAsync(request);
            if (pluginBuildEntity == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Unable to create build."
                });
            }
            return Ok(new GenericResponse<PluginBuildEntity>
            {
                Success = true,
                Message = "Created plugin build.",
                Data = pluginBuildEntity
            });
        }

        [AllowAnonymous]
        [EnableCors("anyorigin")]
        [HttpGet("{pluginId}/{versionCode}")]
        public async Task<IActionResult> GetPluginBuildAsync(int pluginId, int versionCode)
        {
            var pluginBuildEntity = await _pluginBuildService.GetPluginBuildAsync(pluginId, versionCode);
            if (pluginBuildEntity == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Plugin build does not exist."
                });
            }
            return Ok(new GenericResponse<PluginBuildEntity>
            {
                Success = true,
                Data = pluginBuildEntity
            });
        }
        
        [AllowAnonymous]
        [EnableCors("anyorigin")]
        [HttpGet("{pluginId}/page/{pageNumber}")]
        public async Task<IActionResult> GetPluginBuildListPaginatedAsync(int pluginId, int pageNumber)
        {
            var pluginBuildEntities = await _pluginBuildService.PluginBuildsPaginatedAsync(pluginId, pageNumber);
            if (pluginBuildEntities == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Internal server error."
                });
            }

            return Ok(new GenericResponse<List<PluginBuildEntity>>
            {
                Success = true,
                Data = pluginBuildEntities
            });
        }
        
        [Authorize]
        [HttpDelete("{pluginId}/{versionCode}")]
        public async Task<IActionResult> DeletePluginBuildAsync(int pluginId, int versionCode)
        {
            var pluginBuild = await _pluginBuildService.GetPluginBuildAsync(pluginId, versionCode);
            if (pluginBuild == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Plugin build for that pluginId and/or version do not exist."
                });
            }
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            if (!await _pluginService.UserIdOwnsPluginIdAsync(userId, pluginId))
            {
                return Unauthorized(new GenericResponse
                {
                    Success = false,
                    Message = "Unauthorized to delete plugin build."
                });
            }

            await _pluginBuildService.DeletePluginBuildAsync(pluginBuild);
            return Ok(new GenericResponse
            {
                Success = true,
                Message = "Deleted plugin build."
            });
        }
    }
}