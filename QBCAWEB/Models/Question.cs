using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QBCAWEB.Models
{
    public class Question
    {
        [Key]
        public int QuestionID { get; set; }
        public string? QuestionText { get; set; }
        // ... other properties of Question ... (e.g., Answer, Options, etc.)

        // Foreign Key to Subject
        public int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject? Subject { get; set; }

        // Foreign Key to QuestionDifficulty
        public int? DifficultyID { get; set; } // Nullable if a question might not have a difficulty assigned yet
        [ForeignKey("DifficultyID")]
        public virtual QuestionDifficulty? Difficulty { get; set; }

        // You might also have properties like:
        // public DateTime CreatedDate { get; set; }
        // public int CreatedByUserID { get; set; } // FK to User who created it
        // [ForeignKey("CreatedByUserID")]
        // public virtual User? CreatedByUser {get; set;}
    }
}