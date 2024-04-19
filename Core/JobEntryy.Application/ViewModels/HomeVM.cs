using JobEntryy.Domain.Entities;

namespace JobEntryy.Application.ViewModels
{
    public class HomeVM
    {
        public FilterVM Filter { get; set; }
        public List<Job> Jobs { get; set; }
        public int JobsCount { get; set; }
        public List<Category> Categories { get; set; }
        public List<City> Cities { get; set; }
        public List<Experience> Experiences { get; set; }
        public List<JobType> JobTypes { get; set; }
    }
}
