using System.ComponentModel.DataAnnotations;

namespace QBCAWEB.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please select role")]
        public string SelectedRole { get; set; }

        public bool RememberMe { get; set; }
    }
}