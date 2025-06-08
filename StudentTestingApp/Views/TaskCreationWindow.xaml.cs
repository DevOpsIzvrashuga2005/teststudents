using System.Windows;
using StudentTestingApp.Models;

namespace StudentTestingApp.Views
{
    public partial class TaskCreationWindow : Window
    {
        public TaskCreationWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleBox.Text.Trim();
            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Title is required.");
                return;
            }

            var context = ((App)Application.Current).Db;
            var task = new ProgrammingTask
            {
                Title = title,
                Description = DescriptionBox.Text.Trim()
            };
            task.TestCases.Add(new TaskTestCase
            {
                Input = InputBox.Text,
                ExpectedOutput = OutputBox.Text
            });

            context.ProgrammingTasks.Add(task);
            context.SaveChanges();
            DialogResult = true;
            Close();
        }
    }
}
