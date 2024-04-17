using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using JobEntryy.Persistence.Concrete;
using JobEntryy.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;

namespace JobEntryy.Persistence.EntityFramework
{
    public class JobReadRepository : ReadRepository<Job>, IJobReadRepository
    {
        public async Task<int> CompanyJobCountAsync(int userId)
        {
            using var context = new Context();

            int jobCount = await context.Jobs.Where(x => x.UserId == userId && x.Status).CountAsync();
            return jobCount;
        }

        public async Task<List<Job>> GetCompanyJobsLoadMoreAsync(int userId, int skipCount, int take)
        {
            using var context = new Context();

            List<Job> jobs = await context.Jobs.Where(x => x.UserId == userId && x.Status).OrderByDescending(x => x.CreatedTime).
                Skip(skipCount).Take(take).ToListAsync();
            return jobs;
        }

        public async Task<List<Job>> GetCompanyJobsWithTakeAsync(int userId, int take)
        {
            using var context = new Context();

            List<Job> jobs = await context.Jobs.Where(x=>x.UserId==userId && x.Status).OrderByDescending(x=>x.CreatedTime).
                Take(take).ToListAsync();
            return jobs;
        }

        public async Task<List<Job>> GetJobsAsync(int take, int? userId, int? typeId, int? catId, int? cityId, int? expId, string search)
        {
            using var context = new Context();

            IQueryable<Job> jobs = context.Jobs.Include(x=>x.User).Where(x => x.Status).OrderByDescending(x => x.DeadLine).AsQueryable();
            if (userId is not null)
                jobs = jobs.Where(x => x.UserId == userId);
            if(typeId is not null)
                jobs = jobs.Where(x=>x.JobTypeId == typeId);
            if (catId is not null)
                jobs = jobs.Where(x => x.CategoryId == catId);
            if (cityId is not null)
                jobs = jobs.Where(x => x.CityId == cityId);
            if (expId is not null)
                jobs = jobs.Where(x => x.ExperienceId == expId);
            if (search is not null)
                jobs = jobs.Where(x => x.JobName.Contains(search));

            return await jobs.ToListAsync();
        }
    }
}
