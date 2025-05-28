using Microsoft.AspNetCore.Mvc;
using QBCAWEB.Models; 
using Microsoft.Extensions.Logging; 
using Microsoft.AspNetCore.Authorization; 
using System.Diagnostics; 

namespace QBCAWEB.Controllers
{
    [Authorize] 
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Home page accessed");
            ViewBag.Title = "QBCAWEB - Trang chủ";
            ViewBag.Message = "Chào mừng đến với QBCAWEB!";
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Title = "Giới thiệu";
            ViewBag.Message = "Đây là trang giới thiệu về QBCAWEB";
            
            return View();
        }

        [AllowAnonymous] 
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            
        }
    }
}