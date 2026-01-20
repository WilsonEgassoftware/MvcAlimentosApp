namespace SupermarketAPI.DTOs
{
    public class CartItemDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
        public string? ImageUrl { get; set; }
        public int AvailableStock { get; set; }
    }

    public class AddToCartDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateCartItemDTO
    {
        public int Quantity { get; set; }
    }

    public class CartSummaryDTO
    {
        public List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
    }
}
