using JobEntryy.Application.ViewModels;

namespace JobEntryy.Application.Abstract
{
    public interface ICompanyReadRepository
    {
        Task<CompanyVM> GetCompanyAsync(string username);

    }
}
