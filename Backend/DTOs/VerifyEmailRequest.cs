using System.ComponentModel.DataAnnotations;

namespace SupermarketAPI.DTOs
{
    public class VerifyEmailRequest
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
