using System;

namespace StudentTestingApp.Models
{
    public class Submission
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int ProgrammingTaskId { get; set; }
        public ProgrammingTask? ProgrammingTask { get; set; }
        public string Code { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public bool Success { get; set; }
        public string? Output { get; set; }
    }
}
