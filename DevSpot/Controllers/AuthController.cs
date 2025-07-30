using DevSpot.Models.Dtos;
using DevSpot.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevSpot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult>RegisterUser(RegisterDto dto)
        {
            if (await _authService.RegisterUser(dto))
                return Ok(new {message = "Successfully Done!"});
            else
                return BadRequest("Something went wrong!");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto) 
        {
            var token = await _authService.Login(dto);
            if (token != null)
            {
                // Set HttpOnly cookie
                Response.Cookies.Append("token", token, new CookieOptions
                {
                    HttpOnly = true,  // ✅ JS can't read it
                    Secure = true,    // ✅ only sent via HTTPS
                    SameSite = SameSiteMode.None, // ✅ allow cross-site cookies
                    Expires = DateTime.UtcNow.AddMinutes(15) // match token expiry
                });

                return Ok();
            } 
            return Unauthorized();
        }
    }
}
