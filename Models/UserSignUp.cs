using System.ComponentModel.DataAnnotations;

namespace MAPay.Models
{
    public class UserSignUp
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }
        //public IFormFile File { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime LastUpdated { get; set; }
        public string FilePath { get; set; }
        public ICollection<DocumentUpload> documentUploads { get; set; }
    }
}
