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
                return Ok(new { token, userName=dto.Email });
            } 
            return Unauthorized();
        }
    }
}
