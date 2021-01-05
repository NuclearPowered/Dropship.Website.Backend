using System.Security.Claims;
using Dropship.Website.Backend.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dropship.Website.Backend.Controllers
{
    public class ExampleController : ControllerBase
    {
        // An example authenticated request.
        [Authorize]
        [HttpGet("example")]
        public IActionResult Example()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            var userName = HttpContext.User.FindFirstValue(ClaimTypes.Name);

            return Ok(new GenericResponse
            {
                Message = $"Hello {userName}, your user id is {userId}."
            });
        }
    }
}