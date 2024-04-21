
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

        public async Task<List<CompanyVM>> GetAllCompaniesWithPageAsync(int take, int page, string search)
        {
            using var context = new Context();

            List<AppUser> company = await userManager.Users.Where(x=>x.UserRole.Contains("Company") 
            && (search==null || x.Name.Contains(search)))
            .OrderByDescending(x => x.IsPremium).ThenByDescending(x=>x.Jobs.Count)
            .Skip((page-1)*take).Take(take).ToListAsync();
            List<CompanyVM> companyVMs = new List<CompanyVM>();

            foreach (AppUser item in company)
            {
                CompanyVM vm = new CompanyVM
                {
                    Id = item.Id,
                    Name = item.Name,
                    Image = item.Image,
                    UserName = item.UserName,
                    Email = item.Email,
                    CompanyDescription = item.CompanyDescription,
                    WebUrl = item.WebUrl,
                    PhoneNumber = item.PhoneNumber,
                    Address = item.Address,
                    IsPremium = item.IsPremium,
                    Status = item.Status,
                    Created = item.Created,
                    JobsCount = await context.Jobs.Where(x => x.UserId == item.Id).CountAsync(),
                };
                companyVMs.Add(vm);
            }
            return companyVMs;
        }

        public async Task<int> GetAllCompanyCountAsync()
        {
            int count = await userManager.Users.Where(x => x.UserRole.Contains("Company")).CountAsync();
            return count;
        }

        public async Task<double> GetAllCompanyPageCountAsync(double take)
        {
            double pageCount = Math.Ceiling(await userManager.Users.Where(x=>x.UserRole.Contains("Company")).CountAsync() / take);
            return pageCount;
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

        public async Task<List<CompanyVM>> GetPremiumCompaniesWithPageAsync(int take, int page, string search)
        {
            using var context = new Context();

            List<AppUser> company = await userManager.Users.Where(x => x.UserRole.Contains("Company")
            && (search == null || x.Name.Contains(search)) && x.IsPremium)
            .OrderByDescending(x => x.IsPremium).ThenByDescending(x => x.Jobs.Count)
            .Skip((page - 1) * take).Take(take).ToListAsync();
            List<CompanyVM> companyVMs = new List<CompanyVM>();

            foreach (AppUser item in company)
            {
                CompanyVM vm = new CompanyVM
                {
                    Id = item.Id,
                    Name = item.Name,
                    Image = item.Image,
                    UserName = item.UserName,
                    Email = item.Email,
                    CompanyDescription = item.CompanyDescription,
                    WebUrl = item.WebUrl,
                    PhoneNumber = item.PhoneNumber,
                    Address = item.Address,
                    IsPremium = item.IsPremium,
                    Status = item.Status,
                    Created = item.Created,
                    JobsCount = await context.Jobs.Where(x => x.UserId == item.Id).CountAsync(),
                };
                companyVMs.Add(vm);
            }
            return companyVMs;
        }

        public async Task<double> GetPremiumCompanyPageCountAsync(double take)
        {
            double pageCount = Math.Ceiling(await userManager.Users.Where(x => x.UserRole.Contains("Company") && x.IsPremium).CountAsync() / take);
            return pageCount;
        }

        public async Task<int> GetPremiumCountAsync()
        {
            int count = await userManager.Users.Where(x => x.UserRole.Contains("Company") && x.IsPremium).CountAsync();
            return count;
        }
    }
}
