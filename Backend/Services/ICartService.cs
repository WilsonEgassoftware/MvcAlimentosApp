using SupermarketAPI.DTOs;

namespace SupermarketAPI.Services
{
    public interface ICartService
    {
        Task<CartSummaryDTO> GetCartAsync(int userId);
        Task<CartItemDTO> AddToCartAsync(int userId, AddToCartDTO dto);
        Task<bool> UpdateCartItemAsync(int userId, int productId, UpdateCartItemDTO dto);
        Task<bool> RemoveFromCartAsync(int userId, int productId);
        Task<bool> ClearCartAsync(int userId);
    }
}
