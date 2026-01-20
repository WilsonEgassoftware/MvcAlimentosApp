namespace SupermarketAPI.DTOs
{
    public class SupplierDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Contact { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }

    public class CreateSupplierDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Contact { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
