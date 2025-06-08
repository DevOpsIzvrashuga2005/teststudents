using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using StudentTestingApp.Models;

namespace StudentTestingApp.Views
{
    public partial class TaskListWindow : Window
    {
        private readonly StudentTestingContext _context;
        private readonly User _currentUser;

        public TaskListWindow()
        {
            InitializeComponent();
            _context = ((App)Application.Current).Db;
            _currentUser = ((App)Application.Current).CurrentUser!;

            TasksListBox.ItemsSource = _context.ProgrammingTasks.ToList();

            if (_currentUser.RoleId == 2)
            {
                CreateTaskButton.Visibility = Visibility.Visible;
            }
        }

        private void TasksListBox_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TasksListBox.SelectedItem is ProgrammingTask task)
            {
                var editor = new CodeEditorWindow(task);
                editor.ShowDialog();
            }
        }

        private void CreateTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var createWindow = new TaskCreationWindow();
            if (createWindow.ShowDialog() == true)
            {
                TasksListBox.ItemsSource = _context.ProgrammingTasks.ToList();
            }
        }
    }
}
