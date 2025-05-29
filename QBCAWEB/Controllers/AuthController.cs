using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using QBCAWEB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using QBCAWEB.Data;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace QBCAWEB.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

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
                var dbUser = await _context.Users
                                     .Include(u => u.UserRole)
                                     .FirstOrDefaultAsync(u => u.Email == model.Username);

                if (dbUser != null)
                {
                    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(model.Password, dbUser.PasswordHash);

                    if (isPasswordValid)
                    {
                        if (dbUser.UserRole == null)
                        {
                            string roleConfigError = "User account is not configured correctly with a valid role.";
                            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                            {
                                return Json(new { success = false, message = roleConfigError });
                            }
                            ModelState.AddModelError("", roleConfigError);
                            return View(model);
                        }

                        string roleNameFromDb = dbUser.UserRole.RoleName;

                        if (roleNameFromDb.Equals(model.SelectedRole, StringComparison.OrdinalIgnoreCase))
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, dbUser.Email),
                                new Claim(ClaimTypes.Email, dbUser.Email),
                                new Claim(ClaimTypes.Role, roleNameFromDb),
                                new Claim("FullName", dbUser.FullName ?? string.Empty),
                                new Claim("UserID", dbUser.UserID.ToString())
                            };
                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var authProperties = new AuthenticationProperties
                            {
                                IsPersistent = model.RememberMe,
                                ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddMinutes(60)
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
                        string credentialsErrorMessage = "Invalid email or password.";
                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            return Json(new { success = false, message = credentialsErrorMessage });
                        }
                        ModelState.AddModelError("", credentialsErrorMessage);
                    }
                }
                else
                {
                    string credentialsErrorMessage = "Invalid email or password.";
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
            return RedirectToAction("Login");
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
                ViewBag.Message = "If an account with that email address exists, instructions to reset your password have been sent. Please check your inbox (and spam folder).";
                return View("ForgotPasswordConfirmation", model);
            }
            return View(model);
        }

        public IActionResult ForgotPasswordConfirmation(ForgotPasswordViewModel model)
        {
            var displayModel = model ?? new ForgotPasswordViewModel();
            ViewBag.ConfirmationMessage = $"If an account exists for {displayModel.Email}, password reset instructions have been sent. Please check your inbox (and spam folder).";
            return View(displayModel);
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