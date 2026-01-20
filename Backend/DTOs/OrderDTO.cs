namespace SupermarketAPI.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? PaymentMethod { get; set; }
        public string? TransactionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; } = new List<OrderDetailDTO>();
    }

    public class OrderDetailDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
    }

    public class CheckoutRequest
    {
        public string? CardNumber { get; set; } // Nullable para aceptar cualquier valor ficticio
        public string? CardHolderName { get; set; }
        public string? ExpiryDate { get; set; }
        public string? CVV { get; set; }
        public string PaymentMethod { get; set; } = "CreditCard";
    }

    public class CheckoutResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int? OrderId { get; set; }
        public string? TransactionId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
