using JobEntryy.Application.Repositories;
using JobEntryy.Domain.Entities;
using System.Linq.Expressions;

namespace JobEntryy.Application.Abstract
{
    public interface IJobReadRepository : IReadRepository<Job>
    {
        Task<List<Job>> GetCompanyJobsWithTakeAsync(int userId, int take);
        Task<List<Job>> GetCompanyIncludeJobsWithTakeAsync(int userId, int take);
        Task<List<Job>> GetCompanyJobsLoadMoreAsync(int userId,int skipCount, int take);
        Task<int> CompanyJobCountAsync(int userId);

        Task<Job> GetJobWithIncludeAsync(int? id);

        Task<List<Job>> GetJobsAsync(int take, int? typeId, int? catId, int? cityId, int? expId, string search);
        Task<int> GetJobsCountAsync(int take, int? typeId, int? catId, int? cityId, int? expId, string search);

    }
}
