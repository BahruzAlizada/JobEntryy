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

        public async Task<List<CompanyVM>> GetActiveCompaniesAsync(string search, int take)
        {
            using var context = new Context();

            IQueryable<AppUser> users  = userManager.Users.Where(x=>x.Status && x.UserRole.Contains("Company")).
                OrderByDescending(x=>x.IsPremium).ThenByDescending(x=>x.Jobs.Count).AsQueryable();

            if (!string.IsNullOrEmpty(search))
                users = users.Where(x => x.Name.Contains(search));

            List<AppUser> userList = await users.Take(take).ToListAsync();
            List<CompanyVM> companyVMs = new List<CompanyVM>();
            foreach (var item in userList)
            {
                CompanyVM vm = new CompanyVM
                {
                    Id = item.Id,
                    Image = item.Image,
                    Name = item.Name,
                    UserName = item.UserName,
                    IsPremium = item.IsPremium,
                    JobsCount = await context.Jobs.Where(x=>x.Status && x.UserId==item.Id).CountAsync()
                };
                companyVMs.Add(vm);
            }

            return companyVMs;
        }

        public async Task<int> GetActiveCompaniesCountAsync(string search)
        {

            IQueryable<AppUser> users = userManager.Users.Where(x => x.Status && x.UserRole.Contains("Company")).AsQueryable();
            if (!string.IsNullOrEmpty(search))
                users = users.Where(x => x.Name.Contains(search));

            int companyCount = await users.CountAsync();

            return companyCount;
        }

        public async Task<List<CompanyVM>> GetActiveLoadMoreCompaniesAsync(string search, int skipCount, int take)
        {
            using var context = new Context();

            IQueryable<AppUser> users = userManager.Users.Where(x => x.Status && x.UserRole.Contains("Company")).
                OrderByDescending(x => x.IsPremium).ThenByDescending(x => x.Jobs.Count).AsQueryable();

            if (!string.IsNullOrEmpty(search))
                users = users.Where(x => x.Name.Contains(search));

            List<AppUser> userList = await users.Skip(skipCount).Take(take).ToListAsync();
            List<CompanyVM> companies = new List<CompanyVM>();

            foreach (var item in userList)
            {
                CompanyVM vm = new CompanyVM
                {
                    Id = item.Id,
                    Image = item.Image,
                    Name = item.Name,
                    UserName = item.UserName,
                    IsPremium = item.IsPremium,
                    JobsCount = await context.Jobs.Where(x => x.Status && x.UserId == item.Id).CountAsync()
                };
                companies.Add(vm);
            };

            return companies;
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
