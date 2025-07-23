using DevSpot.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DevSpot.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<bool> RegisterUser(LoginUser user)
        {
            var identityUser = new IdentityUser
            {
                UserName = user.UserName,
                Email = user.UserName
            };

            var result = await _userManager.CreateAsync(identityUser, user.Password);

            return result.Succeeded;
        }

        public async Task<bool> Login(LoginUser user)
        {
            var identityUser = await _userManager.FindByEmailAsync(user.UserName);
            if (identityUser == null)
                return false;
            return await _userManager.CheckPasswordAsync(identityUser, user.Password);
        }

        public string GenerateTokenString(LoginUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.UserName),
                new Claim(ClaimTypes.Role, "Admin"),
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims:claims, 
                expires: DateTime.Now.AddSeconds(60),
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                signingCredentials: signingCred
                );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }
    }
}
