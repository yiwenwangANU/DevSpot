using DevSpot.Models;

namespace DevSpot.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(LoginUser user);
    }
}