
using JobEntryy.Application.Abstract;
using JobEntryy.Application.ViewModels;
using JobEntryy.Domain.Identity;
using JobEntryy.Persistence.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobEntryy.Persistence.EntityFramework.Company
{
    public class CompanyReadRepository : ICompanyReadRepository
    {
        private readonly UserManager<AppUser> userManager;
        public CompanyReadRepository(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<CompanyVM> GetCompanyAsync(string username)
        {
            AppUser? user = await userManager.FindByNameAsync(username);
            using var context = new Context();

            CompanyVM company = new CompanyVM
            {
                Id = user.Id,
                Image = user.Image,
                Name = user.Name,
                IsPremium = user.IsPremium,
                CompanyDescription = user.CompanyDescription,
                Address = user.Address,
                WebUrl = user.WebUrl,
                JobsCount = await context.Jobs.Where(x=>x.UserId==user.Id).CountAsync()
            };

            return company;
        }
    }
}
