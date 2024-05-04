using JobEntryy.Application.Abstract;
using JobEntryy.Application.ViewModels;
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

        public async Task<List<Job>> GetAllJobsWithPageAsync(int? companyId, int? typeId, int? catId, int? cityId, int? expId, int take, int page)
        {
            using var context = new Context();

            IQueryable<Job> jobs = context.Jobs.Include(x => x.User).Include(x=>x.Category).
                Include(x=>x.JobType).Include(x=>x.City).Include(x=>x.Experience).Include(x=>x.JobDetail).
                OrderByDescending(x => x.IsPremium).ThenByDescending(x => x.CreatedTime).AsQueryable();
            if (typeId is not null)
                jobs = jobs.Where(x => x.JobTypeId == typeId);
            if (catId is not null)
                jobs = jobs.Where(x => x.CategoryId == catId);
            if (cityId is not null)
                jobs = jobs.Where(x => x.CityId == cityId);
            if (expId is not null)
                jobs = jobs.Where(x => x.ExperienceId == expId);

            return await jobs.Skip((page - 1) * take).Take(take).ToListAsync();
        }

        public async Task<List<Job>> GetCompanyIncludeJobsWithTakeAsync(int userId, int take)
        {
            using var context = new Context();

            List<Job> jobs = await context.Jobs.Include(x=>x.User).Where(x => x.UserId == userId && x.Status).OrderByDescending(x => x.IsPremium).ThenByDescending(x=>x.CreatedTime).
                Take(take).ToListAsync();
            return jobs;
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

        public async Task<int> GetDeadlineFnishedCountAsync()
        {
            using var context = new Context();

            int count = await context.Jobs.Where(x => x.DeadLine.Date < DateTime.Today.Date).CountAsync();
            return count;
        }

        public async Task<List<Job>> GetDeadlineFnsihedJobsAsync(int take, int page)
        {
            using var context = new Context();

            List<Job> jobs = await context.Jobs.Where(x => x.DeadLine.Date < DateTime.Today.Date).Include(x => x.User).Include(x => x.Category).
                Include(x => x.JobType).Include(x => x.City).Include(x => x.Experience).Include(x => x.JobDetail).
                OrderByDescending(x => x.IsPremium).ThenByDescending(x => x.CreatedTime).
                Skip((page - 1) * take).Take(take).ToListAsync();
            return jobs;
        }

        public async Task<double> GetFnishedDeadlinedPageCountAsync(double take)
        {
            using var context = new Context();

            double pageCount = Math.Ceiling(await context.Jobs.Where(x => x.DeadLine.Date < DateTime.Today.Date).CountAsync() / take);
            return pageCount;
        }

        public async Task<List<Job>> GetJobsAsync(FilterVM filter, int take)
        {
            using var context = new Context();

            IQueryable<Job> jobs = context.Jobs.Where(x => x.Status).Include(x => x.User).OrderByDescending(x => x.IsPremium).
                ThenByDescending(x=>x.CreatedTime).Take(take).AsQueryable();

            if(filter.typeId is not null)
                jobs = jobs.Where(x=>x.JobTypeId == filter.typeId);
            if (filter.catId is not null)
                jobs = jobs.Where(x => x.CategoryId == filter.catId);
            if (filter.cityId is not null)
                jobs = jobs.Where(x => x.CityId == filter.cityId);
            if (filter.expId is not null)
                jobs = jobs.Where(x => x.ExperienceId == filter.expId);

            return await jobs.Where(x => (filter.search == null || x.JobName.Contains(filter.search))).ToListAsync();
        }

        public async Task<int> GetJobsCountAsync(FilterVM filter)
        {
            using var context = new Context();

            IQueryable<Job> jobs = context.Jobs.Where(x => x.Status).AsQueryable();

            if (filter.typeId is not null)
                jobs = jobs.Where(x => x.JobTypeId == filter.typeId);
            if (filter.catId is not null)
                jobs = jobs.Where(x => x.CategoryId == filter.catId);
            if (filter.cityId is not null)
                jobs = jobs.Where(x => x.CityId == filter.cityId);
            if (filter.expId is not null)
                jobs = jobs.Where(x => x.ExperienceId == filter.expId);
            
            int jobsCount = await jobs.Where(x => (filter.search == null || x.JobName.Contains(filter.search))).CountAsync();
            return jobsCount;
        }

        public async Task<Job> GetJobWithIncludeAsync(int? id)
        {
            using var context = new Context();

            Job job = await context.Jobs.Include(x => x.User).Include(x => x.JobType).Include(x => x.Category).Include(x => x.City).
                Include(x => x.Experience).Include(x=>x.JobDetail).FirstOrDefaultAsync(x => x.Id == id);
            return job;
        }

        public async Task<List<Job>> GetLoadMoreJobsAsync(FilterVM filter, int skipCount, int take)
        {
            using var context = new Context();

            IQueryable<Job> jobs = context.Jobs.Where(x => x.Status).Include(x => x.User).OrderByDescending(x => x.IsPremium).
                ThenByDescending(x => x.CreatedTime).Skip(skipCount).Take(take).AsQueryable();

            if (filter.typeId is not null)
                jobs = jobs.Where(x => x.JobTypeId == filter.typeId);
            if (filter.catId is not null)
                jobs = jobs.Where(x => x.CategoryId == filter.catId);
            if (filter.cityId is not null)
                jobs = jobs.Where(x => x.CityId == filter.cityId);
            if (filter.expId is not null)
                jobs = jobs.Where(x => x.ExperienceId == filter.expId);
           
            return await jobs.Where(x=>(filter.search==null || x.JobName.Contains(filter.search))).ToListAsync();
        }

        public async Task<List<Job>> GetPremiumJobsWithPageAsync(int take, int page)
        {
            using var context = new Context();

            List<Job> jobs = await context.Jobs.Where(x=>x.IsPremium).Include(x => x.User).Include(x => x.Category).
                Include(x => x.JobType).Include(x => x.City).Include(x => x.Experience).Include(x => x.JobDetail).
                OrderByDescending(x => x.IsPremium).ThenByDescending(x => x.CreatedTime).
                Skip((page-1)*take).Take(take).ToListAsync();
            return jobs;
        }
    }
}
