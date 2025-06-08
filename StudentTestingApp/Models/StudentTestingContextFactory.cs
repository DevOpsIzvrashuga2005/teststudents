using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StudentTestingApp.Models
{
    public class StudentTestingContextFactory : IDesignTimeDbContextFactory<StudentTestingContext>
    {
        public StudentTestingContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StudentTestingContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Database=testing;Username=postgres;Password=secret");
            return new StudentTestingContext(optionsBuilder.Options);
        }
    }
}
