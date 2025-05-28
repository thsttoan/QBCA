using Microsoft.AspNetCore.Mvc;

namespace YourProjectName.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public IActionResult Index()
        {
            return View();
        }
    }
}