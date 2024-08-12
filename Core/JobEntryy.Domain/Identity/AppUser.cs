using JobEntryy.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace JobEntryy.Domain.Identity
{
    public class AppUser : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public string? UserRole { get; set; }

        public string? Image { get; set; }
        public string? CompanyDescription { get; set; }
        public string? WebUrl { get; set; }
        public string? Address { get; set; }
        public bool IsPremium { get; set; }

        public ICollection<Job> Jobs { get; set; }

        public bool Status { get; set; } = true;
        public DateTime Created { get; set; } = DateTime.UtcNow.AddHours(4);
        public DateTime? Updated { get; set; }
    }
}
