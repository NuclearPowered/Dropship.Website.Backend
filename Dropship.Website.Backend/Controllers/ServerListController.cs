using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Dropship.Website.Backend.Database.Entities;
using Dropship.Website.Backend.Models.Requests.ServerList;
using Dropship.Website.Backend.Models.Responses;
using Dropship.Website.Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Dropship.Website.Backend.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ServerListController : ControllerBase
    {
        private readonly ServerListService _srv;

        public ServerListController(ServerListService service)
        {
            _srv = service;
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<IActionResult> CreateUpdateServerAsync(CreateUpdateServerRequest request)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            var server = await _srv.CreateUpdateServerAsync(request, userId);
            if (server == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Could not create server."
                });
            }
            return Ok(new GenericResponse<ServerEntity>
            {
                Success = true,
                Data = server
            });
        }
        
        [Authorize]
        [HttpPost("{serverId}/star")]
        public async Task<IActionResult> UpdateStarForServerId(int serverId)
        {
            if (await _srv.GetServerByIdAsync(serverId) == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Server with that Id does not exist."
                });
            }

            var server = await _srv.UpdateServerStarAsync(serverId);
            return Ok(new GenericResponse<ServerEntity>
            {
                Success = true,
                Message = "Added star to server.",
                Data = server
            });
        }

        [AllowAnonymous]
        [EnableCors("anyorigin")]
        [HttpGet("{serverId}")]
        public async Task<IActionResult> GetServerByIdAsync(int serverId)
        {
            var serverEntity = await _srv.GetServerByIdAsync(serverId);
            if (serverEntity == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "No server with that id."
                });
            }
            return Ok(new GenericResponse<ServerEntity>
            {
                Success = true,
                Data = serverEntity
            });
        }

        [AllowAnonymous]
        [EnableCors("anyorigin")]
        [HttpGet("page/{pageNumber}")]
        public async Task<IActionResult> GetServersListPaginatedAsync(int pageNumber)
        {
            var serverListPaginated = await _srv.GetServerListPaginatedAsync(pageNumber);
            if (serverListPaginated == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Internal server error."
                });
            }

            return Ok(new GenericResponse<List<ServerEntity>>
            {
                Success = true,
                Data = serverListPaginated
            });
        }
        
        [AllowAnonymous]
        [EnableCors("anyorigin")]
        [HttpGet("search")]
        public async Task<IActionResult> SearchServerListAsync([FromQuery] string query)
        {
            if (query == null)
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Search query cannot be empty"
                });
            var serverList = await _srv.SearchServerList(query);
            if (serverList == null)
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Internal server error."
                });

            return Ok(new GenericResponse<List<ServerEntity>>
            {
                Success = true,
                Data = serverList
            });
        }

        [Authorize]
        [HttpDelete("{serverId}")]
        public async Task<IActionResult> DeleteServerAsync(int serverId)
        {
            var serverEntity = await _srv.GetServerByIdAsync(serverId);
            if (serverEntity == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = $"Server with id {serverId} does not exist."
                });
            }

            var userId = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            if (!_srv.UserIdOwnsServer(userId, serverEntity))
            {
                return Unauthorized(new GenericResponse
                {
                    Success = false,
                    Message = "Unauthorized to delete server."
                });
            }
            
            await _srv.DeleteServerAsync(serverEntity);
            return Ok(new GenericResponse
            {
                Success = true,
                Message = "Deleted server."
            });
        }
    }
}