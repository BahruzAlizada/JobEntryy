
using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using JobEntryy.Persistence.Concrete;
using JobEntryy.Persistence.Repositories;

namespace JobEntryy.Persistence.EntityFramework
{
    public class ExperienceWriteRepository : WriteRepository<Experience>, IExperienceWriteRepository
    {
        public void Activity(Experience experience)
        {
            using var context = new Context();

            if (experience.Status)
                experience.Status = false;
            else
                experience.Status=true;

            context.Experiences.Update(experience);
            context.SaveChanges();
        }
    }
}
