using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QBCAWEB.Models
{
    public class CLO
    {
        [Key]
        public int CLOID { get; set; } // Primary Key

        [Required(ErrorMessage = "CLO description cannot be empty.")]
        [StringLength(500, ErrorMessage = "CLO description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        // Foreign Key to Subject
        public int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject? Subject { get; set; } // Navigation property
    }
}