using Microsoft.EntityFrameworkCore;
using SupermarketAPI.Data;
using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> CreateAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<decimal> GetTotalSalesAsync()
        {
            return await _context.Orders
                .Where(o => o.Status == "Completed")
                .SumAsync(o => o.TotalAmount);
        }

        public async Task<int> GetTotalOrdersCountAsync()
        {
            return await _context.Orders.CountAsync();
        }

        public async Task<decimal> GetTodaySalesAsync()
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            return await _context.Orders
                .Where(o => o.Status == "Completed" && 
                           o.CreatedAt >= today && 
                           o.CreatedAt < tomorrow)
                .SumAsync(o => o.TotalAmount);
        }

        public async Task<decimal> GetMonthSalesAsync()
        {
            var firstDayOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);

            return await _context.Orders
                .Where(o => o.Status == "Completed" && 
                           o.CreatedAt >= firstDayOfMonth && 
                           o.CreatedAt < firstDayOfNextMonth)
                .SumAsync(o => o.TotalAmount);
        }

        public async Task<decimal> GetAverageOrderValueAsync()
        {
            var completedOrders = await _context.Orders
                .Where(o => o.Status == "Completed")
                .ToListAsync();

            if (completedOrders.Count == 0)
            {
                return 0;
            }

            return completedOrders.Average(o => o.TotalAmount);
        }
    }
}
