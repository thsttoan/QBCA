// Models/SubjectPlanAssignment.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QBCAWEB.Models
{
    // Optional: [Table("SubjectPlanAssignments")]
    public class SubjectPlanAssignment
    {
        [Key]
        public int AssignmentID { get; set; } // Khóa chính của bảng phân công

        // Foreign Key to ExamPlan
        public int ExamPlanID { get; set; }
        [ForeignKey("ExamPlanID")]
        public virtual ExamPlan? ExamPlan { get; set; }

        // Foreign Key to Subject
        public int SubjectId { get; set; } // Lưu ý tên này, sẽ khớp với FK trong Subject.cs
        [ForeignKey("SubjectId")]
        public virtual Subject? Subject { get; set; } // Navigation property đến Subject

        [Required(ErrorMessage = "Number of questions to collect is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of questions must be at least 1.")]
        [Display(Name = "Target Questions")]
        public int NumberOfQuestionsToCollect { get; set; }

        [Display(Name = "Collected Questions")]
        [Range(0, int.MaxValue)]
        public int NumberOfQuestionsCollected { get; set; } = 0;
    }
}