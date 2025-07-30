using DevSpot.Constants;
using DevSpot.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data;
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

        public async Task<bool> RegisterUser(RegisterDto dto)
        {
            var identityUser = new IdentityUser
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(identityUser, dto.Password);
            if (!result.Succeeded)
                return false;

            var role = dto.IsJobSeeker ? Roles.JOB_SEEKER : Roles.EMPLOYER;
            var roleResult = await _userManager.AddToRoleAsync(identityUser, role);
            if (!roleResult.Succeeded)
                return false;

            return true;
        }

        public async Task<string?> Login(LoginDto dto)
        {
            var identityUser = await _userManager.FindByEmailAsync(dto.Email);
            if (identityUser == null || !await _userManager.CheckPasswordAsync(identityUser, dto.Password))
                return null;
            var userRoles = await _userManager.GetRolesAsync(identityUser);
            return GenerateTokenString(identityUser, userRoles);
        }

        private string GenerateTokenString(IdentityUser user, IList<string> userRoles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
            };
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
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
