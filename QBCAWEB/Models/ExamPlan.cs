// Models/ExamPlan.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QBCAWEB.Models
{
    public enum PlanStatus
    {
        Draft,
        Active,
        Completed,
        Cancelled
    }

    public class ExamPlan
    {
        [Key]
        public int PlanID { get; set; }

        [Required(ErrorMessage = "Plan name is required.")]
        [StringLength(150, ErrorMessage = "Plan name cannot exceed 150 characters.")]
        [Display(Name = "Plan Name")]
        public string? PlanName { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Status")]
        public PlanStatus Status { get; set; } = PlanStatus.Draft;

        // Navigation property: A plan has many SubjectPlanAssignments
        // SỬA LẠI DÒNG NÀY:
        public virtual ICollection<SubjectPlanAssignment> SubjectAssignments { get; set; }

        public ExamPlan()
        {
            // VÀ SỬA LẠI DÒNG NÀY:
            SubjectAssignments = new HashSet<SubjectPlanAssignment>();
        }
    }
}