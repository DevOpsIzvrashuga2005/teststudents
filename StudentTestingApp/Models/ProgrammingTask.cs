namespace StudentTestingApp.Models
{
    public class ProgrammingTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<TaskTestCase> TestCases { get; set; } = new List<TaskTestCase>();
        public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
    }
}
