using System.ComponentModel.DataAnnotations;
using System.Collections.Generic; // Added for ICollection

namespace QBCAWEB.Models
{
    public class QuestionDifficulty
    {
        [Key]
        public int DifficultyID { get; set; } // Primary Key

        [Required(ErrorMessage = "Difficulty level name cannot be empty.")]
        [StringLength(50, ErrorMessage = "Difficulty level name cannot exceed 50 characters.")]
        public string? LevelName { get; set; } // E.g., "Easy", "Medium", "Hard", "Very Hard"

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
        public string? Description { get; set; }

        // A difficulty level can be applied to many questions
        public virtual ICollection<Question>? Questions { get; set; }

        public QuestionDifficulty()
        {
            Questions = new HashSet<Question>();
        }
    }
}