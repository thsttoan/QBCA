using System.ComponentModel.DataAnnotations;

namespace QBCAWEB.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn vai trò.")]
        public string SelectedRole { get; set; }

        public bool RememberMe { get; set; }
    }
}