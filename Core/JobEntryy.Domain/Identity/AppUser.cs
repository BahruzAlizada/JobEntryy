using JobEntryy.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace JobEntryy.Domain.Identity
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public string? UserRole { get; set; }

        public bool Status { get; set; } = true;
        public DateTime Created { get; set; } = DateTime.UtcNow.AddHours(4);
    }
}
