using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using StudentTestingApp.Models;
using StudentTestingApp.Services;

namespace StudentTestingApp.Views
{
    public partial class CodeEditorWindow : Window
    {
        private readonly ProgrammingTask _task;

        public CodeEditorWindow(ProgrammingTask task)
        {
            InitializeComponent();
            // ensure test cases are loaded from the database
            var context = ((App)Application.Current).Db;
            _task = context.ProgrammingTasks
                .Include(t => t.TestCases)
                .FirstOrDefault(t => t.Id == task.Id) ?? task;
            _task = task;
            Title = task.Title;
            DescriptionBlock.Text = task.Description;
            CodeTextBox.Text =
@"using System;

public class Program
{
    public static void Main(string[] args)
    {
        // Write your code here
    }
}";
            CodeTextBox.Focus();
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            var code = CodeTextBox.Text;
            var evaluator = new CodeEvaluator();
            var results = await evaluator.EvaluateAsync(code, _task.TestCases);
            bool success = results.All(r => r.Success);

            var context = ((App)Application.Current).Db;
            var submission = new Submission
            {
                UserId = ((App)Application.Current).CurrentUser!.Id,
                ProgrammingTaskId = _task.Id,
                Code = code,
                SubmittedAt = DateTime.UtcNow,
                Success = success,
                Output = string.Join("\n", results.Select(r => r.Output))
            };
            context.Submissions.Add(submission);
            context.SaveChanges();

            MessageBox.Show(success ? "All tests passed." : "Some tests failed.");
        }
    }
}
