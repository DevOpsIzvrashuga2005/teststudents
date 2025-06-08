using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using StudentTestingApp.Models;

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

        private void ToggleThemeButton_Click(object sender, RoutedEventArgs e)
        {
            ITheme theme = _paletteHelper.GetTheme();
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
