using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using JobEntryy.Persistence.Concrete;
using JobEntryy.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JobEntryy.Persistence.EntityFramework
{
    public class JobReadRepository : ReadRepository<Job>, IJobReadRepository
    {
        public async Task<int> CompanyJobCountAsync(int userId)
        {
            using var context = new Context();

            int jobCount = await context.Jobs.Where(x => x.UserId == userId).CountAsync();
            return jobCount;
        }
    }
}
