using System.Collections.Generic;

namespace StudentTestingApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public Role? Role { get; set; }
        public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
    }
}
