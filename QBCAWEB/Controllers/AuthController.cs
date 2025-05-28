using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using QBCAWEB.Models; // Đảm bảo namespace này đúng và chứa cả LoginViewModel, ForgotPasswordViewModel
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace QBCAWEB.Controllers
{
    public class AuthController : Controller
    {
        private static readonly Dictionary<string, (string Password, UserInfo Info)> DemoUsers =
            new Dictionary<string, (string, UserInfo)>(StringComparer.OrdinalIgnoreCase)
        {
            { "rnd@example.com", ("rnd123", new UserInfo { Username = "rnd@example.com", Email = "rnd@example.com", Role = "rd-staff", FullName = "R&D Staff Member" }) },
            { "lecturer@example.com", ("lecturer123", new UserInfo { Username = "lecturer@example.com", Email = "lecturer@example.com", Role = "lecturer", FullName = "Lecturer User" }) },
            { "hod@example.com", ("hod123", new UserInfo { Username = "hod@example.com", Email = "hod@example.com", Role = "head-dept", FullName = "Head of Department" }) },
            { "leader@example.com", ("leader123", new UserInfo { Username = "leader@example.com", Email = "leader@example.com", Role = "subject-leader", FullName = "Subject Leader User" }) }
        };

        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (IsValidUser(model.Username, model.Password, out UserInfo userInfo))
                {
                    if (userInfo.Role.Equals(model.SelectedRole, StringComparison.OrdinalIgnoreCase))
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, userInfo.Username),
                            new Claim(ClaimTypes.Email, userInfo.Email),
                            new Claim(ClaimTypes.Role, userInfo.Role),
                            new Claim("FullName", userInfo.FullName)
                        };
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = false,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
                        };
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            return Json(new { success = true, message = "Login successful!", returnUrl = Url.Action("Index", "Home") });
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        string roleErrorMessage = "Selected role is incorrect for this account.";
                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            return Json(new { success = false, message = roleErrorMessage });
                        }
                        ModelState.AddModelError("", roleErrorMessage);
                    }
                }
                else
                {
                    string credentialsErrorMessage = "Invalid username or password.";
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = credentialsErrorMessage });
                    }
                    ModelState.AddModelError("", credentialsErrorMessage);
                }
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" && !ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = errors.FirstOrDefault() ?? "Please provide all required information and select the correct role." });
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        public IActionResult Register()
        {
            
            return View(); 
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel()); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Implement logic to handle password reset request.
                // 1. Check if model.Email exists in your user store (DemoUsers or database).
                //    var userExists = DemoUsers.ContainsKey(model.Email); // Ví dụ với DemoUsers
                // 2. If exists:
                //    a. Generate a unique password reset token.
                //    b. Store the token with an expiry time, associated with the user.
                //    c. Send an email to model.Email with a link containing this token.
                //       The link should point to a new ResetPassword action.
                // 3. For security, always show a generic success message, regardless of whether the email exists or not.

                // Ví dụ:
                // bool emailExists = DemoUsers.ContainsKey(model.Email);
                // if (emailExists)
                // {
                //     // Logic to generate token and send email
                //     Console.WriteLine($"Password reset requested for: {model.Email}. Token generation and email sending logic needed.");
                // }

                // Thông báo chung cho người dùng
                ViewBag.Message = "If an account with that email address exists, instructions to reset your password have been sent. Please check your inbox (and spam folder).";
                // Chuyển hướng đến trang xác nhận hoặc hiển thị thông báo trên cùng view
                return View("ForgotPasswordConfirmation", model); // Cần tạo view Views/Auth/ForgotPasswordConfirmation.cshtml
            }

            // Nếu ModelState không hợp lệ, hiển thị lại form với lỗi
            return View(model);
        }

        // Action (GET) để hiển thị trang xác nhận sau khi gửi yêu cầu quên mật khẩu (tùy chọn)
        public IActionResult ForgotPasswordConfirmation(ForgotPasswordViewModel model) // Nhận model để có thể hiển thị lại email nếu muốn
        {
            // Bạn có thể truyền thông báo qua ViewBag nếu không muốn dùng lại model
            // ViewBag.UserEmail = model?.Email; // Chỉ để ví dụ cách truyền lại email
            ViewBag.ConfirmationMessage = $"If an account exists for {model?.Email}, password reset instructions have been sent. Please check your inbox (and spam folder).";
            return View(); // Sẽ render Views/Auth/ForgotPasswordConfirmation.cshtml
        }
        // --- KẾT THÚC PHẦN SỬA ĐỔI VÀ BỔ SUNG ---


        private bool IsValidUser(string username, string password, out UserInfo userInfo)
        {
            userInfo = null;
            if (DemoUsers.TryGetValue(username, out var userData))
            {
                if (userData.Password == password)
                {
                    userInfo = userData.Info;
                    return true;
                }
            }
            return false;
        }
    }


    public class UserInfo
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }
}