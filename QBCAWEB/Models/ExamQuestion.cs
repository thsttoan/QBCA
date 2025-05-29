namespace QBCAWEB.Models
{
    public class ExamQuestion
    {
        public int ExamId { get; set; }
        public int QuestionId { get; set; }
        
        public double Percentage { get; set; } // Difficulty percentage
        
        // Navigation properties
        public virtual ExamPlan Exam { get; set; } = null!;
        public virtual Question Question { get; set; } = null!;
    }
}