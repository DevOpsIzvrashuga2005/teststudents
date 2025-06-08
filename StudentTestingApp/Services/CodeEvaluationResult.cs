namespace StudentTestingApp.Services
{
    public class CodeEvaluationResult
    {
        public bool Success { get; set; }
        public string Output { get; set; } = string.Empty;
        public string Errors { get; set; } = string.Empty;
    }
}
