using System.Security.Claims;
using System.Threading.Tasks;
using Dropship.Website.Backend.Models.Requests.Auth;
using Dropship.Website.Backend.Models.Responses;
using Dropship.Website.Backend.Models.Responses.Auth;
using Dropship.Website.Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Dropship.Website.Backend.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [EnableCors("anyorigin")]
        [HttpPost("login")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticateRequest request)
        {
            // TODO: Model validation.

            var user = await _authService.AuthenticateAsync(request);
            if (user == null)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Invalid username/password combination."
                });
            }

            // Generate JWT token for frontend.
            var token = _authService.GenerateJwt(user);

            return Ok(new GenericResponse<AuthenticateResponse>
            {
                Success = true,
                Data = new AuthenticateResponse
                {
                    Token = token
                }
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            // TODO: Model validation, proper responses.

            if (await _authService.UsernameExistsAsync(request.Username))
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Username already in use."
                });
            }

            if (await _authService.EmailExistsAsync(request.Email))
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Message = "Email already in use."
                });
            }

            await _authService.RegisterAsync(request);

            return Ok(new GenericResponse
            {
                Success = true,
                Message = "Registered successfully."
            });
        }
        
        [Authorize]
        [HttpPost("usernameFree")]
        public async Task<IActionResult> IsUsernameFreeAsync(string username)
        {
            if (await _authService.UsernameExistsAsync(username))
            {
                return Ok(new GenericResponse
                {
                    Success = false,
                    Message = "Username is in use."
                });
            }

            return Ok(new GenericResponse
            {
                Success = true,
                Message = "Username is not take."
            });
        }
        
        [Authorize]
        [HttpPost("updatePassword")]
        public async Task<IActionResult> UpdatePasswordAsync([FromBody] UpdatePasswordRequest request)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.Sid);

            await _authService.ChangePasswordAsync(int.Parse(userId), request.Password);

            return Ok(new GenericResponse
            {
                Success = true,
                Message = "Password Updated"
            });
        }
    }
}