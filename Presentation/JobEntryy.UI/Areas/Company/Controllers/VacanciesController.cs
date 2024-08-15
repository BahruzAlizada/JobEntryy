using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using JobEntryy.Domain.Identity;
using JobEntryy.Persistence.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace JobEntryy.UI.Areas.Company.Controllers
{
    [Area("Company")]
    public class VacanciesController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IJobReadRepository jobReadRepository;
        private readonly IJobWriteRepository jobWriteRepository;
        private readonly ICategoryReadRepository categoryReadRepository;
        private readonly ICityReadRepository cityReadRepository;
        private readonly IJobTypeReadRepository jobTypeReadRepository;
        private readonly IExperienceReadRepository experienceReadRepository;
        public VacanciesController(UserManager<AppUser> userManager, IJobReadRepository jobReadRepository, IJobWriteRepository jobWriteRepository,
            ICategoryReadRepository categoryReadRepository,ICityReadRepository cityReadRepository, 
            IJobTypeReadRepository jobTypeReadRepository,IExperienceReadRepository experienceReadRepository)
        {
            this.userManager = userManager;
            this.jobReadRepository = jobReadRepository;
            this.jobWriteRepository = jobWriteRepository;
            this.categoryReadRepository = categoryReadRepository;
            this.cityReadRepository = cityReadRepository;
            this.jobTypeReadRepository = jobTypeReadRepository;
            this.experienceReadRepository = experienceReadRepository;
        }

        #region Index
        public async Task<IActionResult> Index()
        {
            AppUser? user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return BadRequest();

            ViewBag.UserId = user.Id;
            ViewBag.JobsCount = await jobReadRepository.CompanyJobCountAsync(user.Id);

            List<Job> jobs = await jobReadRepository.GetCompanyIncludeJobsWithTakeAsync(user.Id, take:15);
            return View(jobs);
        }
        #endregion

        #region ListMore
        public async Task<IActionResult> ListMore(Guid userId, int skipCount)
        {
            int companyJobsCount = await jobReadRepository.CompanyJobCountAsync(userId);

            if (companyJobsCount <= skipCount)
                return Content("d");

            List<Job> jobs = await jobReadRepository.GetCompanyJobsLoadMoreAsync(userId, skipCount, take:15);
            return PartialView("_ListMore", jobs);
        }
        #endregion

        #region Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await categoryReadRepository.GetActiveCachingCategories();
            ViewBag.Cities = await cityReadRepository.GetActiveCachingCities();
            ViewBag.Experiences = await experienceReadRepository.GetAllAsync(x => x.Status);
            ViewBag.JobTypes = await jobTypeReadRepository.GetAllAsync(x => x.Status);

            AppUser? user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return BadRequest();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Job job, Guid catId, Guid cityId, Guid expId, Guid typeId)
        {
            using var context = new Context();

            ViewBag.Categories = await categoryReadRepository.GetActiveCachingCategories();
            ViewBag.Cities = await cityReadRepository.GetActiveCachingCities();
            ViewBag.Experiences = await experienceReadRepository.GetAllAsync(x => x.Status);
            ViewBag.JobTypes = await jobTypeReadRepository.GetAllAsync(x => x.Status);

            AppUser? user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return BadRequest();

            job.UserId = user.Id;
            job.JobTypeId = typeId;
            job.CategoryId = catId;
            job.CityId = cityId;
            job.ExperienceId = expId;

            if (user.IsPremium)
            {
                job.IsPremium = true;
                job.PremiumDate = DateTime.UtcNow.AddDays(2);
            }

            job.DeadLine = DateTime.UtcNow.AddDays(30);

            if (job.JobDetail.Link is null && job.JobDetail.Email is null)
            {
                ModelState.AddModelError("", "Email və Link xanalarından sadəcə birini doldura bilərsiniz");
                return View();
            }
            if (job.JobDetail.Link is not null && job.JobDetail.Email is not null)
            {
                ModelState.AddModelError("", "Email və Link xanalarından sadəcə bir xananı doldurun");
                return View();
            }


            await context.Jobs.AddAsync(job);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
