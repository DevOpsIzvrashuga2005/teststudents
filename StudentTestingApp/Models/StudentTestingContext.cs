using Microsoft.EntityFrameworkCore;

namespace StudentTestingApp.Models
{
    public class StudentTestingContext : DbContext
    {
        public StudentTestingContext(DbContextOptions<StudentTestingContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<ProgrammingTask> ProgrammingTasks => Set<ProgrammingTask>();
        public DbSet<TaskTestCase> TaskTestCases => Set<TaskTestCase>();
        public DbSet<Submission> Submissions => Set<Submission>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Student" },
                new Role { Id = 2, Name = "Teacher" }
            );
        }
    }
}
