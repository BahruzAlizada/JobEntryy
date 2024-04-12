using JobEntryy.Application.Repositories;
using JobEntryy.Domain.Entities;

namespace JobEntryy.Application.Abstract
{
    public interface IJobTypeWriteRepository : IWriteRepository<JobType>
    {
        void Activity(JobType jobType);
    }
}
