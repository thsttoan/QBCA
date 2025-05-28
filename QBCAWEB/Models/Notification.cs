using System.ComponentModel.DataAnnotations;

namespace QBCAWEB.Models
{
    public class Notification
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; } = string.Empty;
        
        public string? Content { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public bool IsRead { get; set; } = false;
        
        public string? UserId { get; set; }
    }
}