using JobEntryy.Application.ViewModels;

namespace JobEntryy.Application.Abstract
{
    public interface ICompanyReadRepository
    {
        Task<CompanyVM> GetCompanyAsync(string username);


        Task<double> GetAllCompanyPageCountAsync(double take);
        Task<int> GetAllCompanyCountAsync();
        Task<List<CompanyVM>> GetAllCompaniesWithPageAsync(int take, int page, string search);

        Task<double> GetPremiumCompanyPageCountAsync(double take);
        Task<int> GetPremiumCountAsync();
        Task<List<CompanyVM>> GetPremiumCompaniesWithPageAsync(int take, int page, string search);


        Task<List<CompanyVM>> GetActiveCompaniesAsync(string search, int take); //*
        Task<int> GetActiveCompaniesCountAsync(string search); //*
        Task<List<CompanyVM>> GetActiveLoadMoreCompaniesAsync(string search, int skipCount, int take);
    }
}
