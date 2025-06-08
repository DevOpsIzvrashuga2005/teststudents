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
                var options = new DbContextOptionsBuilder<StudentTestingContext>()
                    .UseNpgsql("Host=localhost;Database=testing;Username=postgres;Password=539571")
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
