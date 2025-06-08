using System.Windows;
using System.Windows.Controls;
using StudentTestingApp.Models;

namespace StudentTestingApp.Views
{
    public partial class TaskCreationWindow : Window
    {
        private int _caseCounter = 0;

        public TaskCreationWindow()
        {
            InitializeComponent();
            AddTestCaseRow();
        }

        private void AddTestCaseRow()
        {
            var panel = new StackPanel { Margin = new Thickness(0,0,0,10) };
            panel.Children.Add(new TextBlock { Text = $"Input" });
            var inputBox = new TextBox { Margin = new Thickness(0,0,0,5) };
            panel.Children.Add(inputBox);
            panel.Children.Add(new TextBlock { Text = "Expected Output" });
            var outputBox = new TextBox();
            panel.Children.Add(outputBox);
            TestCasesPanel.Children.Add(panel);
        }

        private void AddTestCaseButton_Click(object sender, RoutedEventArgs e)
        {
            AddTestCaseRow();
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

            foreach (StackPanel panel in TestCasesPanel.Children)
            {
                if (panel.Children.Count >= 4 &&
                    panel.Children[1] is TextBox input &&
                    panel.Children[3] is TextBox output)
                {
                    task.TestCases.Add(new TaskTestCase
                    {
                        Input = input.Text,
                        ExpectedOutput = output.Text
                    });
                }
            }

            context.ProgrammingTasks.Add(task);
            context.SaveChanges();
            DialogResult = true;
            Close();
        }
    }
}
