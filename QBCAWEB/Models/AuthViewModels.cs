﻿using System.ComponentModel.DataAnnotations;

namespace QBCAWEB.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
    }
}