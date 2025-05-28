using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QBCAWEB.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Column("UserID")]
        public int UserID { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty; 

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [Column("RoleID")]
        public int RoleID { get; set; }

        
        [ForeignKey("RoleID")]
        public virtual Role? UserRole { get; set; } 

        [Column("CreatedAt")]
        public DateTime? CreatedAt { get; set; }

        public bool? IsActive { get; set; }

    }
}