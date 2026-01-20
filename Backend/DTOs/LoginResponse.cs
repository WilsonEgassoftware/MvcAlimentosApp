namespace SupermarketAPI.DTOs
{
    public class LoginResponse
    {
        public string? Token { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int UserId { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string? Message { get; set; }
    }
}
