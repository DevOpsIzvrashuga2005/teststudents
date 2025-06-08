using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StudentTestingApp.Models
{
    public class StudentTestingContextFactory : IDesignTimeDbContextFactory<StudentTestingContext>
    {
        public StudentTestingContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StudentTestingContext>();
            string conn = Environment.GetEnvironmentVariable("CONNECTION_STRING")

                ?? "Host=localhost;Database=testing;Username=postgres;Password=secret";
            optionsBuilder.UseNpgsql(conn);
            return new StudentTestingContext(optionsBuilder.Options);
        }
    }
}
