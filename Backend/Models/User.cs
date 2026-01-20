using System.ComponentModel.DataAnnotations;

namespace SupermarketAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = "User"; // "Admin" o "User"

        [StringLength(200)]
        public string? FullName { get; set; }

        public bool IsEmailConfirmed { get; set; } = false;

        [StringLength(255)]
        public string? EmailVerificationToken { get; set; }

        public DateTime? EmailVerificationTokenExpiry { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }

        // Relación inversa con Orders
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
