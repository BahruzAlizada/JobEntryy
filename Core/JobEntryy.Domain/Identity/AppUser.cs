using JobEntryy.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace JobEntryy.Domain.Identity
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public string? UserRole { get; set; }

        public string? Image { get; set; }
        public string? CompanyDescription { get; set; }
        public string? WebUrl { get; set; }
        public string? Address { get; set; }
        public bool IsPremium { get; set; }

        public List<Job> Jobs { get; set; }

        public bool Status { get; set; } = true;
        public DateTime Created { get; set; } = DateTime.UtcNow.AddHours(4);
    }
}
