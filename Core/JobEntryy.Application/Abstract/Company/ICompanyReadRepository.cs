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
    }
}
