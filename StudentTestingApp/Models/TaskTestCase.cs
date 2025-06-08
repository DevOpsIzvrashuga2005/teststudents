namespace StudentTestingApp.Models
{
    public class TaskTestCase
    {
        public int Id { get; set; }
        public int ProgrammingTaskId { get; set; }
        public ProgrammingTask? ProgrammingTask { get; set; }
        public string Input { get; set; } = string.Empty;
        public string ExpectedOutput { get; set; } = string.Empty;
    }
}
