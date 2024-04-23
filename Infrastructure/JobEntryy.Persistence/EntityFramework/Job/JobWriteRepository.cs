
using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using JobEntryy.Persistence.Concrete;
using JobEntryy.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JobEntryy.Persistence.EntityFramework
{
    public class JobWriteRepository : WriteRepository<Job>, IJobWriteRepository
    {
        public async Task AddPremiumDate(Job job, DateTime date)
        {
            using var context = new Context();

            if (!job.IsPremium)
            {
                job.PremiumDate = date;
                job.IsPremium = true;
            }

            context.Jobs.Update(job);
            context.SaveChanges();
        }

        public async Task DeleteFinishedDeadlineAsync()
        {
            using var context = new Context();

            List<Job> jobs = await context.Jobs.Where(x => x.DeadLine.Day < DateTime.Today.Day).ToListAsync();
            foreach (var item in jobs)
            {
                context.Jobs.Remove(item);
            }

            await context.SaveChangesAsync();
        }

        public void PremiumDelete(Job job)
        {
            using var context = new Context();

            if (job.IsPremium)
            {
                job.IsPremium = false;
                job.PremiumDate = null;
            }

            context.Jobs.Update(job);
            context.SaveChanges();
        }
    }
}
