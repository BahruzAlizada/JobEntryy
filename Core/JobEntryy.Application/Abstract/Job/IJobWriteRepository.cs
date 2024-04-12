using JobEntryy.Application.Repositories;
using JobEntryy.Domain.Entities;

namespace JobEntryy.Application.Abstract
{
    public interface IJobWriteRepository : IWriteRepository<Job>
    {
    }
}
