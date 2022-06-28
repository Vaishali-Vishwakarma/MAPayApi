using System.ComponentModel.DataAnnotations;

namespace MAPay.Models
{
    public class AddUserRequest
    {
        public string Name { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }
        public IFormFile File { get; set; }

        //public List<IFormFile> File { get; set; }
        //public ICollection<DocumentUpload> documentUploads { get; set; }
    }
}
