namespace QBCAWEB.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
        public int NumberOfQuestions { get; set; } // From "Number of questions"
        public double DifficultyPercentage { get; set; } // From "Enter the difficulty"
        public bool IsApproved { get; set; } // From "Approval"
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }
    }
}