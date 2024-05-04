using JobEntryy.Application.Repositories;
using JobEntryy.Application.ViewModels;
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

        Task<List<Job>> GetJobsAsync(FilterVM filter, int take);
        Task<List<Job>> GetLoadMoreJobsAsync(FilterVM filter, int skipCount, int take);
        Task<int> GetJobsCountAsync(FilterVM filter);


        Task<List<Job>> GetAllJobsWithPageAsync(int? companyId, int? typeId, int? catId, int? cityId, int? expId, int take, int page);


        Task<List<Job>> GetPremiumJobsWithPageAsync(int take, int page);


        Task<List<Job>> GetDeadlineFnsihedJobsAsync(int take, int page);
        Task<double> GetFnishedDeadlinedPageCountAsync(double take);
        Task<int> GetDeadlineFnishedCountAsync();

    }
}
