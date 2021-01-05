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
    public class ImagesController : ControllerBase
    {
        private readonly ImageUploadService _srv;

        public ImagesController(ImageUploadService srv)
        {
            _srv = srv;
        }

        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> CreateImageUpload([FromForm] IFormFile image)
        {
            var imageUrl = await _srv.UploadImage(image);
            if (imageUrl == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Could not upload file."
                });
            }

            return Ok(new GenericResponse<string>
            {
                Success = true,
                Data = imageUrl
            });
        }
    }
}