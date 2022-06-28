using System.ComponentModel.DataAnnotations;

namespace MAPay.Models
{
    public class AddDoctorRequest
    {
        public string Name { get; set; }
        public string Role { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
