using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAPay.Models
{
    public class DocumentUpload
    {
        [Key]
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
