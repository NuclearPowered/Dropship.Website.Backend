using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Dropship.Website.Backend.Database.Entities;
using Dropship.Website.Backend.Models.Requests.Mods;
using Dropship.Website.Backend.Models.Responses;
using Dropship.Website.Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Dropship.Website.Backend.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ModsController : ControllerBase
    {
        private readonly ModsService _srv;

        public ModsController(ModsService service)
        {
            _srv = service;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateModAsync(CreateModRequest request)
        {
            if (await _srv.GetModByGuidAsync(request.Guid) != null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Bad Guid to create a new mod."
                });
            }
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            var modEntity = await _srv.CreateModAsync(request, userId);
            return Ok(new GenericResponse<ModEntity>
            {
                Success = true,
                Message = "Created mod.",
                Data = modEntity
            });
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateModAsync(UpdateModRequest request)
        {
            if (await _srv.GetModByIdAsync(request.Id) == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Mod with that Id does not exist."
                });
            }
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            if (!await _srv.UserIdOwnsModIdAsync(userId, request.Id))
            {
                return Unauthorized(new GenericResponse
                {
                    Success = false,
                    Message = "Unauthorized to update mod."
                });
            }
            var modEntity = await _srv.UpdateModAsync(request);
            return Ok(new GenericResponse<ModEntity>
            {
                Success = true,
                Message = "Updated mod.",
                Data = modEntity
            });
        }
        
        [Authorize]
        [HttpPost("{modId}/star")]
        public async Task<IActionResult> UpdateStarForModId(int modId)
        {
            if (await _srv.GetModByIdAsync(modId) == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Mod with that Id does not exist."
                });
            }

            var modEntity = await _srv.UpdateModStarAsync(modId);
            return Ok(new GenericResponse<ModEntity>
            {
                Success = true,
                Message = "Added star to mod.",
                Data = modEntity
            });
        }

        [AllowAnonymous]
        [EnableCors("anyorigin")]
        [HttpGet("{modId}")]
        public async Task<IActionResult> GetModByIdAsync(int modId)
        {
            var modEntity = await _srv.GetModByIdAsync(modId);
            if (modEntity == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Mod with that Id does not exist."
                });
            }
            return Ok(new GenericResponse<ModEntity>
            {
                Success = true,
                Data = modEntity
            });
        }
        
        [AllowAnonymous]
        [EnableCors("anyorigin")]
        [HttpGet("guid/{guid}")]
        public async Task<IActionResult> GetModByGuid(string guid)
        {
            var modEntity = await _srv.GetModByGuidAsync(guid);
            if (modEntity == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Mod does not exist."
                });
            }
            return Ok(new GenericResponse<ModEntity>
            {
                Success = true,
                Data = modEntity
            });
        }

        [AllowAnonymous]
        [EnableCors("anyorigin")]
        [HttpGet("page/{pageNumber}")]
        public async Task<IActionResult> GetModListPaginatedAsync(int pageNumber)
        {
            var modListPaginated = await _srv.GetModListPaginatedAsync(pageNumber);
            if (modListPaginated == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Internal server error."
                });
            }

            return Ok(new GenericResponse<List<ModEntity>>
            {
                Success = true,
                Data = modListPaginated
            });
        }

        [AllowAnonymous]
        [EnableCors("anyorigin")]
        [HttpGet("search")]
        public async Task<IActionResult> SearchModListAsync([FromQuery] string query)
        {
            if (query == null)
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Search query cannot be empty"
                });
            
            var modList = await _srv.SearchModList(query);
            if (modList == null)
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Internal server error."
                });

            return Ok(new GenericResponse<List<ModEntity>>
            {
                Success = true,
                Data = modList
            });
        }

        [Authorize]
        [HttpDelete("{modId}")]
        public async Task<IActionResult> DeleteModAsync(int modId)
        {
            if (await _srv.GetModByIdAsync(modId) == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Mod with that Id does not exist."
                });
            }
            
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            if (!await _srv.UserIdOwnsModIdAsync(userId, modId))
            {
                return Unauthorized(new GenericResponse
                {
                    Success = false,
                    Message = "Unauthorized to delete mod."
                });
            }
            
            await _srv.DeleteModAsync(modId);
            return Ok(new GenericResponse
            {
                Success = true,
                Message = "Deleted mod."
            });
        }
    }
}