using System;
// using System.ComponentModel.DataAnnotations; // Không cần thiết ở đây nếu không có validation attributes

namespace QBCAWEB.Models
{
    public class UserViewModel
    {
        public int UserID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public int RoleID { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}