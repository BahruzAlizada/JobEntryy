

using JobEntryy.Domain.Identity;

namespace JobEntryy.Application.Abstract.Company
{
    public interface ICompanyWriteRepository
    {
        Task ActivityAsync(AppUser company);
        Task PremiumAsync(AppUser company);
    }
}
