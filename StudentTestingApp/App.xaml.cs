using System;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using StudentTestingApp.Models;

namespace StudentTestingApp
{
    public partial class App : Application
    {
        public StudentTestingContext Db { get; private set; } = null!;
        public User? CurrentUser { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                string connString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                    ?? "Host=localhost;Database=testing;Username=postgres;Password=secret";

                var options = new DbContextOptionsBuilder<StudentTestingContext>()
                    .UseNpgsql(connString)
                    .Options;

                Db = new StudentTestingContext(options);
                Db.Database.Migrate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize database: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }
    }
}
