using JobEntryy.Domain.Common;
using JobEntryy.Domain.Identity;
using System.ComponentModel.DataAnnotations;

namespace JobEntryy.Domain.Entities
{
    public class Job : EntityList
    {
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid CityId { get; set; }
        public Guid ExperienceId { get; set; }
        public Guid JobTypeId { get; set; }

        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public string JobName { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public DateTime DeadLine { get; set; }
        public DateTime? PremiumDate { get; set; }
        public bool IsPremium { get; set; }

        public JobDetail JobDetail { get; set; }
        public AppUser User { get; set; }
        public Category Category { get; set; }
        public City City { get; set; }
        public Experience Experience { get; set; }
        public JobType JobType { get; set; }
    }
}
