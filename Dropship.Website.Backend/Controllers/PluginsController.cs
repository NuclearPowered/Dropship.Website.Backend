using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Dropship.Website.Backend.Database.Entities;
using Dropship.Website.Backend.Models.Requests.Plugins;
using Dropship.Website.Backend.Models.Responses;
using Dropship.Website.Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Dropship.Website.Backend.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class PluginsController : ControllerBase
    {
        private readonly PluginService _srv;

        public PluginsController(PluginService service)
        {
            _srv = service;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreatePluginAsync(CreatePluginRequest request)
        {
            if (await _srv.GetPluginByGuidAsync(request.Guid) != null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Bad Guid to create a new plugin."
                });
            }
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            var pluginEntity = await _srv.CreatePluginAsync(request, userId);
            return Ok(new GenericResponse<PluginEntity>
            {
                Success = true,
                Message = "Created plugin.",
                Data = pluginEntity
            });
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<IActionResult> UpdatePluginAsync(UpdatePluginRequest request)
        {
            if (await _srv.GetPluginByIdAsync(request.Id) == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Plugin with that Id does not exist."
                });
            }
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            if (!await _srv.UserIdOwnsPluginIdAsync(userId, request.Id))
            {
                return Unauthorized(new GenericResponse
                {
                    Success = false,
                    Message = "Unauthorized to update plugin."
                });
            }
            var pluginEntity = await _srv.UpdatePluginAsync(request);
            return Ok(new GenericResponse<PluginEntity>
            {
                Success = true,
                Message = "Updated plugin.",
                Data = pluginEntity
            });
        }
        
        [Authorize]
        [HttpPost("{pluginId}/star")]
        public async Task<IActionResult> UpdateStarForPluginId(int pluginId)
        {
            if (await _srv.GetPluginByIdAsync(pluginId) == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Plugin with that Id does not exist."
                });
            }

            var pluginEntity = await _srv.UpdatePluginStarAsync(pluginId);
            return Ok(new GenericResponse<PluginEntity>
            {
                Success = true,
                Message = "Added star to plugin.",
                Data = pluginEntity
            });
        }

        [AllowAnonymous]
        [EnableCors("anyorigin")]
        [HttpGet("{pluginId}")]
        public async Task<IActionResult> GetPluginByIdAsync(int pluginId)
        {
            var pluginEntity = await _srv.GetPluginByIdAsync(pluginId);
            if (pluginEntity == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Plugin with that Id does not exist."
                });
            }
            return Ok(new GenericResponse<PluginEntity>
            {
                Success = true,
                Data = pluginEntity
            });
        }
        
        [AllowAnonymous]
        [EnableCors("anyorigin")]
        [HttpGet("guid/{guid}")]
        public async Task<IActionResult> GetPluginByGuid(string guid)
        {
            var pluginEntity = await _srv.GetPluginByGuidAsync(guid);
            if (pluginEntity == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Plugin does not exist."
                });
            }
            return Ok(new GenericResponse<PluginEntity>
            {
                Success = true,
                Data = pluginEntity
            });
        }

        [AllowAnonymous]
        [EnableCors("anyorigin")]
        [HttpGet("page/{pageNumber}")]
        public async Task<IActionResult> GetPluginListPaginatedAsync(int pageNumber)
        {
            var pluginListPaginated = await _srv.GetPluginListPaginatedAsync(pageNumber);
            if (pluginListPaginated == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Internal server error."
                });
            }

            return Ok(new GenericResponse<List<PluginEntity>>
            {
                Success = true,
                Data = pluginListPaginated
            });
        }
        
        [AllowAnonymous]
        [EnableCors("anyorigin")]
        [HttpGet("search")]
        public async Task<IActionResult> SearchPluginListAsync([FromQuery] string query)
        {
            if (query == null)
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Search query cannot be empty"
                });
            var pluginList = await _srv.SearchPluginList(query);
            if (pluginList == null)
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Internal server error."
                });

            return Ok(new GenericResponse<List<PluginEntity>>
            {
                Success = true,
                Data = pluginList
            });
        }

        [Authorize]
        [HttpDelete("{pluginId}")]
        public async Task<IActionResult> DeletePluginAsync(int pluginId)
        {
            if (await _srv.GetPluginByIdAsync(pluginId) == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Plugin with that Id does not exist."
                });
            }
            
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            if (!await _srv.UserIdOwnsPluginIdAsync(userId, pluginId))
            {
                return Unauthorized(new GenericResponse
                {
                    Success = false,
                    Message = "Unauthorized to delete plugin."
                });
            }
            
            await _srv.DeletePluginAsync(pluginId);
            return Ok(new GenericResponse
            {
                Success = true,
                Message = "Deleted plugin."
            });
        }
    }
}