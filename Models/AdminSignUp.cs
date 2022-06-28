using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MAPay.Models
{
    public class AdminSignUp
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
        public DateTime LastLogin { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
