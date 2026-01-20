using System.ComponentModel.DataAnnotations;

namespace SupermarketAPI.DTOs
{
    public class RegisterRequest
    {
        [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [StringLength(200)]
        public string? FullName { get; set; }

        [StringLength(50)]
        public string Role { get; set; } = "User"; // Por defecto "User", solo Admin puede crear otros Admins
    }
}
