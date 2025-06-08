using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using StudentTestingApp.Models;
using MaterialDesignThemes.Wpf;

namespace StudentTestingApp.Views
{
    public partial class TaskListWindow : Window
    {
        private readonly StudentTestingContext _context;
        private readonly User _currentUser;
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();

        public TaskListWindow()
        {
            InitializeComponent();
            _context = ((App)Application.Current).Db;
            _currentUser = ((App)Application.Current).CurrentUser!;

            try
            {
                TasksListBox.ItemsSource = _context.ProgrammingTasks
                    .Include(t => t.TestCases)
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load tasks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (_currentUser.RoleId == 2)
            {
                CreateTaskButton.Visibility = Visibility.Visible;
            }
        }

        private void TasksListBox_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TasksListBox.SelectedItem is ProgrammingTask task)
            {
                // reload the task including its test cases to ensure they are available
                var loaded = _context.ProgrammingTasks
                    .Include(t => t.TestCases)
                    .FirstOrDefault(t => t.Id == task.Id);
                if (loaded == null)
                    return;
                var editor = new CodeEditorWindow(loaded);
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

        private void ToggleThemeButton_Click(object sender, RoutedEventArgs e)
        {
            var theme = _paletteHelper.GetTheme();
            if (theme.GetBaseTheme() == BaseTheme.Dark)
            {
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                theme.SetBaseTheme(Theme.Dark);
            }
            _paletteHelper.SetTheme(theme);
        }
    }
}
