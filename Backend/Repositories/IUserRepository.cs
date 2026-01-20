using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByVerificationTokenAsync(string token);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
    }
}
