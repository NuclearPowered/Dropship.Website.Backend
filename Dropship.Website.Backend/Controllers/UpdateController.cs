using System.Collections.Generic;
using System.Threading.Tasks;
using Dropship.Website.Backend.Models.Requests.Update;
using Dropship.Website.Backend.Models.Responses;
using Dropship.Website.Backend.Models.Responses.Update;
using Dropship.Website.Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Dropship.Website.Backend.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UpdateController : ControllerBase
    {
        
        private readonly ModBuildsService _modBuildsService;
        private readonly ModsService _modsService;

        public UpdateController(ModBuildsService modBuildsService, ModsService modsService)
        {
            _modBuildsService = modBuildsService;
            _modsService = modsService;
        }

        [AllowAnonymous]
        [EnableCors("anyorigin")]
        [HttpPost("checkmodbuildupdates")]
        public async Task<IActionResult> CheckModBuildUpdates(CheckModBuildUpdateRequest request)
        {
            var modBuildUpdates = await _modsService.GetLatestModBuilds(request);
            if (modBuildUpdates == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "GUIDs do not have mods and/or mod builds associated with them"
                });
            }
            return Ok(new GenericResponse<List<CheckModBuildUpdateResponse>>
            {
                Success = true,
                Data = modBuildUpdates
            });
        }
    }
}