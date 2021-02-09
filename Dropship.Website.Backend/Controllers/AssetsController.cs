using System;
using System.Threading.Tasks;
using Dropship.Website.Backend.Models.Responses;
using Dropship.Website.Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dropship.Website.Backend.Controllers
{
    [Route("api/[controller]")]
    public class AssetsController : ControllerBase
    {
        private readonly UploadService _srv;

        public AssetsController(UploadService srv)
        {
            _srv = srv;
        }

        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> CreateAssetAsync([FromForm] IFormFile asset)
        {
            var assetUrl = await _srv.UploadAsset(asset);
            if (assetUrl == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Could not upload asset."
                });
            }

            return Ok(new GenericResponse<string>
            {
                Success = true,
                Data = assetUrl
            });
        }
    }
}