using JobEntryy.Application.Repositories;
using JobEntryy.Domain.Entities;

namespace JobEntryy.Application.Abstract
{
    public interface IJobWriteRepository : IWriteRepository<Job>
    {
        Task AddPremiumDate(Job job, DateTime date);
        void PremiumDelete(Job job);
        Task DeleteFinishedDeadlineAsync();
    }
}
