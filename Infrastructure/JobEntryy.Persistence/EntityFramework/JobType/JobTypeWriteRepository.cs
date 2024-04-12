
using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using JobEntryy.Persistence.Concrete;
using JobEntryy.Persistence.Repositories;

namespace JobEntryy.Persistence.EntityFramework
{
    public class JobTypeWriteRepository : WriteRepository<JobType>, IJobTypeWriteRepository
    {
        public void Activity(JobType jobType)
        {
            using var context = new Context();

            if (jobType.Status)
                jobType.Status = false;
            else
                jobType.Status = true;

            context.JobTypes.Update(jobType);
            context.SaveChanges();
        }
    }
}
