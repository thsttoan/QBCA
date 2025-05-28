using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using QBCAWEB.Models; 
using System.Collections.Generic;
using System.Linq;
using System;

namespace QBCAWEB.Controllers
{
    [Authorize] 
    public class NotificationController : Controller
    {
        
        private static List<NotificationViewModel> GetSampleNotifications()
        {
            return new List<NotificationViewModel>
            {
                new NotificationViewModel { Id = 1, Title = "New Task Assigned", Message = "You have been assigned to review 15 questions for 'Advanced AI'.", ReceivedDate = DateTime.UtcNow.AddHours(-1), IsRead = false, LinkUrl = "#/tasks/123" },
                new NotificationViewModel { Id = 2, Title = "Subject Plan Approved", Message = "The question plan for 'Software Engineering' has been approved by the Head of Department.", ReceivedDate = DateTime.UtcNow.AddHours(-5), IsRead = false, LinkUrl = "#/plans/details/SE001" },
                new NotificationViewModel { Id = 3, Title = "Exam Submitted", Message = "The final exam for 'Data Structures' has been submitted by the Subject Leader.", ReceivedDate = DateTime.UtcNow.AddDays(-1), IsRead = true, LinkUrl = "#/exams/review/DS002" },
                new NotificationViewModel { Id = 4, Title = "Duplicate Alert Resolved", Message = "Duplicate questions found in 'Introduction to Programming' have been resolved.", ReceivedDate = DateTime.UtcNow.AddDays(-2), IsRead = true },
                new NotificationViewModel { Id = 5, Title = "System Maintenance", Message = "The system will undergo scheduled maintenance tonight from 2 AM to 3 AM.", ReceivedDate = DateTime.UtcNow.AddDays(-3), IsRead = true },
            };
        }
        

        public IActionResult Index()
        {
            
            var notifications = GetSampleNotifications().OrderByDescending(n => n.ReceivedDate).ToList();
            return View(notifications); 
        }

        [HttpPost]
        public IActionResult MarkAsRead(int notificationId)
        {
            
            Console.WriteLine($"Notification {notificationId} marked as read (server-side).");
            
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult MarkAllAsRead()
        {
            Console.WriteLine("All notifications marked as read (server-side).");
            return Json(new { success = true });
        }
    }
}