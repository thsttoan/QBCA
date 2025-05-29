using System.ComponentModel.DataAnnotations;

namespace QBCAWEB.Models
{
    public class UpdateUserDto
    {
        [Required]
        public int UserID { get; set; }

        [MaxLength(100)]
        public string? FullName { get; set; }

        [EmailAddress]
        [MaxLength(255)]
        public string? Email { get; set; }

        public string? RoleName { get; set; }

        public bool? IsActive { get; set; }
    }
}