using JobEntryy.Application.Abstract;
using JobEntryy.Application.ViewModels;
using JobEntryy.Domain.Entities;
using JobEntryy.Domain.Identity;
using JobEntryy.Persistence.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobEntryy.UI.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IJobReadRepository jobReadRepository;
        public CompaniesController(UserManager<AppUser> userManager, IJobReadRepository jobReadRepository)
        {
            this.userManager = userManager;
            this.jobReadRepository = jobReadRepository;
        }

        #region Index
        public async Task<IActionResult> Index(string search)
        {
            int take = 21;
            ViewBag.CompanyCount = await userManager.Users.Where(x => x.Status && (search == null || x.Name.Contains(search)) &&
            x.UserRole.Contains("Company")).CountAsync();

            List<AppUser> users = await userManager.Users.Where(x => x.Status && x.UserRole.Contains("Company")
            && (search == null || x.Name.Contains(search))).OrderByDescending(x => x.IsPremium).ThenByDescending(x=>x.Jobs.Count)
            .Take(take).ToListAsync();
            List<CompanyVM> companies = new List<CompanyVM>();

            foreach (var item in users)
            {
                CompanyVM vm = new CompanyVM
                {
                    Id = item.Id,
                    Image = item.Image,
                    Name = item.Name,
                    UserName = item.UserName,
                    IsPremium = item.IsPremium,
                    JobsCount = await jobReadRepository.CompanyJobCountAsync(item.Id)
                };
                companies.Add(vm);
            }

            return View(companies);
        }
        #endregion

        #region LoadMore
        public async Task<IActionResult> LoadMore(string search, int skipCount)
        {
            int take = 21;
            int companyCount = await userManager.Users.Where(x => x.Status && (search == null || x.Name.Contains(search)) &&
            x.UserRole.Contains("Company")).CountAsync();

            if (companyCount <= skipCount)
                return Content("Ok");

            List<AppUser> users = await userManager.Users.Where(x => x.Status && x.UserRole.Contains("Company")
            && (search == null || x.Name.Contains(search))).OrderByDescending(x => x.IsPremium).ThenByDescending(x => x.Jobs.Count)
            .Skip(skipCount).Take(take).ToListAsync();
            List<CompanyVM> companies = new List<CompanyVM>();

            foreach (var item in users)
            {
                CompanyVM vm = new CompanyVM
                {
                    Id = item.Id,
                    Image = item.Image,
                    Name = item.Name,
                    UserName = item.UserName,
                    IsPremium = item.IsPremium,
                    JobsCount = await jobReadRepository.CompanyJobCountAsync(item.Id)
                };
                companies.Add(vm);
            }

            return PartialView("_CompanyPartialView", companies);

        }
        #endregion



        #region CompanyVacancies
        public async Task<IActionResult> CompanyVacancies(string username)
        {
            if (username == null) return NotFound();
            AppUser? company = await userManager.FindByNameAsync(username);
            if (company == null) return BadRequest();

            ViewBag.JobsCount = await jobReadRepository.CompanyJobCountAsync(company.Id);
            List<Job> jobs = await jobReadRepository.GetCompanyIncludeJobsWithTakeAsync(company.Id, take:15);
            return View(jobs);
        }
        #endregion

        #region CompanyVacanciesLoadMore
        public async Task<IActionResult> CompanyVacanciesLoadMore(Guid userId, int skipCount, int take)
        {
            int companyJobsCount = await jobReadRepository.CompanyJobCountAsync(userId);

            if (companyJobsCount <= skipCount)
                return Content("di");

            List<Job> jobs = await jobReadRepository.GetCompanyJobsLoadMoreAsync(userId, skipCount, take:15);
            return PartialView("_CompanyVacanciesLoadMore", jobs);
        }
        #endregion

     
        #region Detail
        public async Task<IActionResult> Detail(string username)
        {
            AppUser? user = await userManager.FindByNameAsync(username);
            if (user == null) return BadRequest();

            CompanyVM company = new CompanyVM
            {
                Id = user.Id,
                Image = user.Image,
                Name = user.Name,
                UserName = user.UserName,
                IsPremium = user.IsPremium,
                CompanyDescription = user.CompanyDescription,
                Address = user.Address,
                WebUrl = user.WebUrl,
                JobsCount = await jobReadRepository.CompanyJobCountAsync(user.Id)
            };

            return View(company);
        }
        #endregion
    }
}
