using JobEntryy.Domain.Entities;
using JobEntryy.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobEntryy.Persistence.Concrete
{
    public class Context : IdentityDbContext<AppUser,AppRole,int>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-OK3QKVJ;Database=JobEntryyDataBase;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true;Integrated Security=True;");
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobDetail> JobDetails { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Package> Packages { get; set; }
    }
}
