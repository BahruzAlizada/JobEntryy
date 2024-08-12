using JobEntryy.Application.Repositories;
using JobEntryy.Application.ViewModels;
using JobEntryy.Domain.Entities;
using System.Linq.Expressions;

namespace JobEntryy.Application.Abstract
{
    public interface IJobReadRepository : IReadRepository<Job>
    {
        Task<List<Job>> GetCompanyJobsWithTakeAsync(Guid userId, int take);
        Task<List<Job>> GetCompanyIncludeJobsWithTakeAsync(Guid userId, int take);
        Task<List<Job>> GetCompanyJobsLoadMoreAsync(Guid userId,int skipCount, int take);
        Task<int> CompanyJobCountAsync(Guid userId);

        Task<Job> GetJobWithIncludeAsync(Guid? id);

        Task<List<Job>> GetJobsAsync(FilterVM filter, int take);
        Task<List<Job>> GetLoadMoreJobsAsync(FilterVM filter, int skipCount, int take);
        Task<int> GetJobsCountAsync(FilterVM filter);


        Task<List<Job>> GetAllJobsWithPageAsync(Guid? companyId, Guid? typeId, Guid? catId, Guid? cityId, Guid? expId, int take, int page);


        Task<List<Job>> GetPremiumJobsWithPageAsync(int take, int page);


        Task<List<Job>> GetDeadlineFnsihedJobsAsync(int take, int page);
        Task<double> GetFnishedDeadlinedPageCountAsync(double take);
        Task<int> GetDeadlineFnishedCountAsync();

    }
}
