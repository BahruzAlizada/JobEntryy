using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace JobEntryy.UI.Areas.Company.Controllers
{
    [Area("Company")]
    public class VacanciesController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IJobWriteRepository jobWriteRepository;
        private readonly ICategoryReadRepository categoryReadRepository;
        private readonly ICityReadRepository cityReadRepository;
        private readonly IJobTypeReadRepository jobTypeReadRepository;
        private readonly IExperienceReadRepository experienceReadRepository;
        public VacanciesController(UserManager<AppUser> userManager, IJobWriteRepository jobWriteRepository,
            ICategoryReadRepository categoryReadRepository,ICityReadRepository cityReadRepository, 
            IJobTypeReadRepository jobTypeReadRepository,IExperienceReadRepository experienceReadRepository)
        {
            this.userManager = userManager;
            this.jobWriteRepository = jobWriteRepository;
            this.categoryReadRepository = categoryReadRepository;
            this.cityReadRepository = cityReadRepository;
            this.jobTypeReadRepository = jobTypeReadRepository;
            this.experienceReadRepository = experienceReadRepository;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Categories = await categoryReadRepository.GetActiveCachingCategories();
            ViewBag.Cities = await cityReadRepository.GetActiveCachingCities();
            ViewBag.Experiences = await experienceReadRepository.GetAllAsync(x=>x.Status);
            ViewBag.JobTypes = await jobTypeReadRepository.GetAllAsync(x => x.Status);

            AppUser? user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return BadRequest();

            return View();
        }

      
    }
}
