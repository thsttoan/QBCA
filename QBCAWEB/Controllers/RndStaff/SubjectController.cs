using Microsoft.AspNetCore.Mvc;

namespace YourAppName.Controllers.RndStaff
{
    public class SubjectController : Controller
    {
        public IActionResult Subject()
        {
            // Logic to populate ViewBag.Subjects and ViewBag.Notifications if needed
            return View();
        }

        [HttpPost]
        public IActionResult CreateSubject(string subjectName, string clos, string difficultyLevel)
        {
            // Logic to create a subject
            return RedirectToAction("Subject");
        }

        [HttpPost]
        public IActionResult AssignPlan(int subjectId, int questionCount)
        {
            // Logic to assign a question plan
            return RedirectToAction("Subject");
        }
    }
}