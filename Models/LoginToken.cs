using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Models
{
    public class LoginToken
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [Required]
        public string Token { get; set; } = string.Empty;

        public DateTime Expiry { get; set; }

        public bool IsUsed { get; set; } = false;
    }
}
