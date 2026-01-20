using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetByUserIdAsync(int userId);
        Task<Order?> GetByIdAsync(int id);
        Task<Order> CreateAsync(Order order);
        Task<Order> UpdateAsync(Order order);
        Task<decimal> GetTotalSalesAsync();
        Task<int> GetTotalOrdersCountAsync();
        Task<decimal> GetTodaySalesAsync(); // Ventas de hoy
        Task<decimal> GetMonthSalesAsync(); // Ventas del mes actual
        Task<decimal> GetAverageOrderValueAsync(); // Promedio por orden
    }
}
