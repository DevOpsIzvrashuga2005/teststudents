using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using StudentTestingApp.Models;

namespace StudentTestingApp.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }

            try
            {
                StudentTestingContext context = ((App)Application.Current).Db;
                string passwordHash = HashPassword(password);

                var user = context.Users.FirstOrDefault(u => u.UserName == username && u.PasswordHash == passwordHash);
                if (user == null)
                {
                    MessageBox.Show("Invalid credentials.");
                    return;
                }

                ((App)Application.Current).CurrentUser = user;
                var taskWindow = new TaskListWindow();
                Application.Current.MainWindow = taskWindow;
                taskWindow.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during login: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }

            try
            {
                StudentTestingContext context = ((App)Application.Current).Db;

                if (context.Users.Any(u => u.UserName == username))
                {
                    MessageBox.Show("User already exists.");
                    return;
                }

                var newUser = new User
                {
                    UserName = username,
                    PasswordHash = HashPassword(password),
                    RoleId = 1
                };
                context.Users.Add(newUser);
                context.SaveChanges();

                MessageBox.Show("Registration successful. You can now log in.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during registration: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
