

using JobEntryy.Application.Abstract.Company;
using JobEntryy.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace JobEntryy.Persistence.EntityFramework.Company
{
    public class CompanyWriteRepository : ICompanyWriteRepository
    {
        private readonly UserManager<AppUser> userManager;
        public CompanyWriteRepository(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task ActivityAsync(AppUser company)
        {
            if (company.Status)
                company.Status = false;
            else
                company.Status = true;

            await userManager.UpdateAsync(company);
        }

        public async Task PremiumAsync(AppUser company)
        {
            if(company.IsPremium)
                company.IsPremium = false;
            else
                company.IsPremium = true;

            await userManager.UpdateAsync(company);
        }
    }
}
