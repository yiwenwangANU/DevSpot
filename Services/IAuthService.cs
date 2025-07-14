using DevSpot.Models;

namespace DevSpot.Services
{
    public interface IAuthService
    {
        string GenerateTokenString(LoginUser user);
        Task<bool> RegisterUser(LoginUser user);
        Task<bool> Login(LoginUser user);
    }
}