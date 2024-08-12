using JobEntryy.Application.Abstract;
using JobEntryy.Application.ViewModels;
using JobEntryy.Domain.Entities;
using JobEntryy.UI.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace JobEntryy.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IJobReadRepository jobReadRepository;
        private readonly ICategoryReadRepository categoryReadRepository;
        private readonly ICityReadRepository cityReadRepository;
        private readonly IExperienceReadRepository experienceReadRepository;
        private readonly IJobTypeReadRepository jobTypeReadRepository;
        public HomeController(IJobReadRepository jobReadRepository, ICategoryReadRepository categoryReadRepository,
            ICityReadRepository cityReadRepository, IExperienceReadRepository experienceReadRepository,
            IJobTypeReadRepository jobTypeReadRepository)
        {
            this.jobReadRepository = jobReadRepository;
            this.categoryReadRepository = categoryReadRepository;
            this.cityReadRepository = cityReadRepository;
            this.experienceReadRepository = experienceReadRepository;
            this.jobTypeReadRepository = jobTypeReadRepository;
        }

        #region Index
        public async Task<IActionResult> Index(FilterVM filter)
        {
            List<Job> jobs = await jobReadRepository.GetJobsAsync(filter, take:30);
            await ProcessFilters(filter);

            HomeVM homeVM = new HomeVM
            {
                Jobs = jobs,
                JobsCount = await jobReadRepository.GetJobsCountAsync(filter),
                Categories = await categoryReadRepository.GetActiveCachingCategories(),
                Cities = await cityReadRepository.GetActiveCachingCities(),
                Experiences = await experienceReadRepository.GetAllAsync(x => x.Status),
                JobTypes = await jobTypeReadRepository.GetAllAsync(x => x.Status),
                Filter = filter
            };
            return View(homeVM);
        }
        #endregion

        #region LoadMore
        public async Task<IActionResult> LoadMore(int skipCount, string filterr)
        {
            FilterVM filter = JsonConvert.DeserializeObject<FilterVM>(filterr);

            int jobsCount = await jobReadRepository.GetJobsCountAsync(filter);
            if (jobsCount <= skipCount)
                return Content(".");

            List<Job> jobs = await jobReadRepository.GetLoadMoreJobsAsync(filter, skipCount, take: 30);
            return PartialView("_VacancyPartialView", jobs);
        }
        #endregion

        #region Detail
        public async Task<IActionResult> Detail(Guid? id)
        {
            if (id is null) return NotFound();
            Job job = await jobReadRepository.GetJobWithIncludeAsync(id);
            if (job is null) return BadRequest();

            return View(job);
        }
        #endregion

        #region ProcessFilter
        private async Task<IActionResult> ProcessFilters(FilterVM filter)
        {
            if (filter.search is not null)
                ViewBag.SearchName = filter.search;

            if (filter.typeId is not null)
            {
                JobType jobType = await jobTypeReadRepository.GetAsync(x=>x.Id==filter.typeId);
                if (jobType == null) return BadRequest();
                ViewBag.SelectedJobTypeName = jobType.Name;
            }

            if (filter.catId is not null)
            {
                Category category = await categoryReadRepository.GetAsync(x=>x.Id==filter.catId);
                if (category == null) return BadRequest();
                ViewBag.SelectedCategoryName = category.Name;
            }

            if (filter.cityId is not null)
            {
                City city = await cityReadRepository.GetAsync(x=>x.Id==filter.cityId);
                if (city == null) return BadRequest();
                ViewBag.SelectedCityName = city.Name;
            }

            if (filter.expId is not null)
            {
                Experience experience = await experienceReadRepository.GetAsync(x=>x.Id==filter.expId);
                if (experience == null) return BadRequest();
                ViewBag.SelectedExperience = experience.Name;
            }

            
            return View();
        }
        #endregion




        #region ChangeCulture
        public async Task<IActionResult> ChangeCulture(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions()
                {
                    Expires = DateTimeOffset.UtcNow.AddMonths(6)
                });
            return Redirect(Request.Headers["Referer"].ToString());
        }
        #endregion

        #region Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}
