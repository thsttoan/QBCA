namespace QBCAWEB.Models
{
    public abstract class Question
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public int DifficultyLevel { get; set; } // Based on "Question difficulty levels" in the flowchart
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }
    }
}