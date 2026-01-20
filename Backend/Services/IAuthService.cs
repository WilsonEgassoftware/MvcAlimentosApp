using SupermarketAPI.DTOs;

namespace SupermarketAPI.Services
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        Task<LoginResponse?> RegisterAsync(RegisterRequest request);
        Task<bool> VerifyEmailAsync(string token);
    }
}
