using DevSpot.Models.Dtos;
using Microsoft.AspNetCore.Identity;

namespace DevSpot.Services
{
    public interface IAuthService
    {
        string GenerateTokenString(IdentityUser user);
        Task<bool> RegisterUser(LoginDto user);
        Task<string?> Login(LoginDto user);
    }
}