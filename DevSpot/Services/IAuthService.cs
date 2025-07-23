using DevSpot.Models.Dtos;

namespace DevSpot.Services
{
    public interface IAuthService
    {
        string GenerateTokenString(LoginUserDto user);
        Task<bool> RegisterUser(LoginUserDto user);
        Task<bool> Login(LoginUserDto user);
    }
}