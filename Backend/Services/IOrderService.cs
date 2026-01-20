using SupermarketAPI.DTOs;

namespace SupermarketAPI.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAllOrdersAsync();
        Task<IEnumerable<OrderDTO>> GetUserOrdersAsync(int userId);
        Task<OrderDTO?> GetOrderByIdAsync(int id);
        Task<CheckoutResponse> ProcessCheckoutAsync(int userId, CheckoutRequest request);
        Task<bool> SendInvoiceAsync(int orderId, int userId);
    }
}
