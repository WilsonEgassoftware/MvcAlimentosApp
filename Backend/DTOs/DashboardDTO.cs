namespace SupermarketAPI.DTOs
{
    public class DashboardDTO
    {
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
        public int TotalSuppliers { get; set; }
        public int LowStockProducts { get; set; } // Stock < 5
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public decimal TodaySales { get; set; } // Ventas de hoy
        public decimal MonthSales { get; set; } // Ventas del mes actual
        public decimal AverageOrderValue { get; set; } // Promedio por orden
        public List<ProductDTO> LowStockItems { get; set; } = new List<ProductDTO>();
    }
}
