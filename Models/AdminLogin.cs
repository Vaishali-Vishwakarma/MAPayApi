using System.ComponentModel.DataAnnotations;

namespace MAPay.Models
{
    public class AdminLogin
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
    }
}
