using JobEntryy.Application.Abstract;
using JobEntryy.Application.ViewModels;
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
            int take = 15;
            List<AppUser> users = await userManager.Users.Where(x => x.Status && x.UserRole.Contains("Company")
            && (search == null || x.Name.Contains(search))).OrderByDescending(x => x.IsPremium).ThenByDescending(x=>x.Created)
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
