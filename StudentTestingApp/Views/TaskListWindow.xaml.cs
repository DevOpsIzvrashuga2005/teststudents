using System.Collections.Generic;
using System.Windows;
using StudentTestingApp.Models;

namespace StudentTestingApp.Views
{
    public partial class TaskListWindow : Window
    {
        public TaskListWindow()
        {
            InitializeComponent();
            // Placeholder data
            TasksListBox.ItemsSource = new List<ProgrammingTask>
            {
                new ProgrammingTask { Title = "Sample Task 1" },
                new ProgrammingTask { Title = "Sample Task 2" }
            };
        }
    }
}
