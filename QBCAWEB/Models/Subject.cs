// Models/Subject.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Required for [Table] if you use it

namespace QBCAWEB.Models
{
    // Optional: [Table("Subjects")] if you want to explicitly name the table
    public class Subject
    {
        [Key]
        public int Id { get; set; } // Khóa chính của Subject

        [Required(ErrorMessage = "Subject name cannot be empty.")]
        [StringLength(100, ErrorMessage = "Subject name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        [StringLength(20, ErrorMessage = "Subject code cannot exceed 20 characters.")]
        public string? SubjectCode { get; set; }

        public string? Description { get; set; }

        // Navigation properties
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<CLO> CLOs { get; set; }
        public virtual ICollection<SubjectPlanAssignment> PlanAssignments { get; set; } // Liên kết đến các kế hoạch mà môn học này tham gia

        public Subject()
        {
            Questions = new HashSet<Question>();
            CLOs = new HashSet<CLO>();
            PlanAssignments = new HashSet<SubjectPlanAssignment>();
        }
    }
}