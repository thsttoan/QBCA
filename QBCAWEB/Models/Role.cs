using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic; 

namespace QBCAWEB.Models
{
    [Table("Roles")] 
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        [Column("RoleID")] 
        public int RoleID { get; set; }

        [Required]
        [MaxLength(100)] 
        [Column("RoleName")] 
        public string RoleName { get; set; } = string.Empty;

    }
}