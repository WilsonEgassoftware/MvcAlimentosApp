using SupermarketAPI.DTOs;
using SupermarketAPI.Repositories;

namespace SupermarketAPI.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductService _productService;

        public DashboardService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            ISupplierRepository supplierRepository,
            IOrderRepository orderRepository,
            IProductService productService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _supplierRepository = supplierRepository;
            _orderRepository = orderRepository;
            _productService = productService;
        }

        public async Task<DashboardDTO> GetDashboardDataAsync()
        {
            var products = await _productRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();
            var suppliers = await _supplierRepository.GetAllAsync();
            var lowStockProducts = await _productService.GetLowStockProductsAsync(5);
            var totalSales = await _orderRepository.GetTotalSalesAsync();
            var totalOrders = await _orderRepository.GetTotalOrdersCountAsync();
            var todaySales = await _orderRepository.GetTodaySalesAsync();
            var monthSales = await _orderRepository.GetMonthSalesAsync();
            var averageOrderValue = await _orderRepository.GetAverageOrderValueAsync();

            return new DashboardDTO
            {
                TotalProducts = products.Count(),
                TotalCategories = categories.Count(),
                TotalSuppliers = suppliers.Count(),
                LowStockProducts = lowStockProducts.Count(),
                TotalSales = totalSales,
                TotalOrders = totalOrders,
                TodaySales = todaySales,
                MonthSales = monthSales,
                AverageOrderValue = averageOrderValue,
                LowStockItems = lowStockProducts.ToList()
            };
        }
    }
}
