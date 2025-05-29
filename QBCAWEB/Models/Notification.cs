using QBCAWEB.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QBCA.Models
{
    [Table("Notification")]
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        public int UserId { get; set; } // User nhận thông báo
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Link { get; set; } // Link để điều hướng khi click (có thể là hash client-side)
        public string IconClass { get; set; } // Ví dụ: "fas fa-tasks"
    }
}