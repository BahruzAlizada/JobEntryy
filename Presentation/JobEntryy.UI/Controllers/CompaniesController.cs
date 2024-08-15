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
        private readonly ICompanyReadRepository companyReadRepository;
        private readonly IJobReadRepository jobReadRepository;
        public CompaniesController(UserManager<AppUser> userManager, ICompanyReadRepository companyReadRepository, IJobReadRepository jobReadRepository)
        {
            this.userManager = userManager;
            this.companyReadRepository = companyReadRepository;
            this.jobReadRepository = jobReadRepository;
        }

        #region Index
        public async Task<IActionResult> Index(string search)
        {
            ViewBag.CompanyCount = await companyReadRepository.GetActiveCompaniesCountAsync(search);

            List<CompanyVM> companies = await companyReadRepository.GetActiveCompaniesAsync(search, take: 21);
            return View(companies);
        }
        #endregion

        #region LoadMore
        public async Task<IActionResult> LoadMore(string search, int skipCount)
        {
            int companyCount = await companyReadRepository.GetActiveCompaniesCountAsync(search);

            if (companyCount <= skipCount)
                return Content("Ok");

            List<CompanyVM> companies = await companyReadRepository.GetActiveLoadMoreCompaniesAsync(search, skipCount, take: 21);
            return PartialView("_CompanyPartialView", companies);

        }
        #endregion



        #region CompanyVacancies
        public async Task<IActionResult> CompanyVacancies(Guid? id)
        {
            if (id == null) return NotFound();
            AppUser? company = await userManager.Users.FirstOrDefaultAsync(x=>x.Id==id);
            if (company == null) return BadRequest();

            ViewBag.UserId = company.Id;
            ViewBag.JobsCount = await jobReadRepository.CompanyJobCountAsync(company.Id);

            List<Job> jobs = await jobReadRepository.GetCompanyIncludeJobsWithTakeAsync(company.Id, take:3);
            return View(jobs);
        }
        #endregion

        #region CompanyVacanciesLoadMore
        public async Task<IActionResult> CompanyVacanciesLoadMore(string userId, int skipCount, int take)
        {
            Guid companyId = Guid.Parse(userId);
            int companyJobsCount = await jobReadRepository.CompanyJobCountAsync(companyId);
            Console.WriteLine(companyJobsCount);
            if (companyJobsCount <= skipCount)
                return Content("di");

            List<Job> jobs = await jobReadRepository.GetCompanyJobsLoadMoreAsync(companyId, skipCount, take:3);
            return PartialView("_CompanyVacanciesLoadMore", jobs);
        }
        #endregion

     
        #region Detail
        public async Task<IActionResult> Detail(Guid? id)
        {
            if (id == null) return NotFound();
            AppUser? user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
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
