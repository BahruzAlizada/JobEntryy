using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using JobEntryy.Persistence.Concrete;
using JobEntryy.Persistence.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobEntryy.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,JobManager,ContactManager")]
    public class VacancyController : Controller
    {
        private readonly IJobReadRepository jobReadRepository;
        private readonly IJobWriteRepository jobWriteRepository;
        private readonly IJobTypeReadRepository jobTypeReadRepository;
        private readonly ICategoryReadRepository categoryReadRepository;
        private readonly ICityReadRepository cityReadRepository;
        private readonly IExperienceReadRepository experienceReadRepository;
        public VacancyController(IJobReadRepository jobReadRepository, IJobWriteRepository jobWriteRepository,
            IJobTypeReadRepository jobTypeReadRepository, ICategoryReadRepository categoryReadRepository,
            ICityReadRepository cityReadRepository, IExperienceReadRepository experienceReadRepository)
        {
            this.jobReadRepository = jobReadRepository;
            this.jobWriteRepository = jobWriteRepository;
            this.jobTypeReadRepository = jobTypeReadRepository;
            this.categoryReadRepository = categoryReadRepository;
            this.cityReadRepository = cityReadRepository;
            this.experienceReadRepository = experienceReadRepository;
        }

        #region Index
        public async Task<IActionResult> Index(int? companyId, int? typeId, int? catId, int? cityId, int? expId, int page = 1)
        {
            double take = 15;
            ViewBag.PageCount = await jobReadRepository.GetPagedCountAsync(take);
            ViewBag.JobCount = await jobReadRepository.GetCountAsync(x => x.Status);

            List<Job> jobs = await jobReadRepository.GetAllJobsWithPageAsync(companyId, typeId, catId, cityId, expId, (int)take, page);
            return View(jobs);
        }
        #endregion

        #region PremiumVacancies
        public async Task<IActionResult> PremiumVacancies(int page = 1)
        {
            double take = 15;
            ViewBag.PageCount = await jobReadRepository.GetPagedCountAsync(take, x => x.IsPremium);
            ViewBag.CurrentPage = page;

            ViewBag.JobCount = await jobReadRepository.GetCountAsync(x => x.IsPremium);
            List<Job> jobs = await jobReadRepository.GetPremiumJobsWithPageAsync((int)take, page);
            return View(jobs);
        }
        #endregion

        #region PastVacancies
        public async Task<IActionResult> PastVacancies(int page = 1)
        {
            double take = 15;
            ViewBag.PageCount = await jobReadRepository.GetFnishedDeadlinedPageCountAsync(take);
            ViewBag.CurrentPage = page;

            ViewBag.JobCount = await jobReadRepository.GetDeadlineFnishedCountAsync();
            List<Job> jobs = await jobReadRepository.GetDeadlineFnsihedJobsAsync((int)take, page);
            return View(jobs);
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(int? id)
        {
            using var context = new Context();

            if (id == null) return NotFound();
            Job? dbjob = await context.Jobs.Include(x => x.JobDetail).FirstOrDefaultAsync(x => x.Id == id);
            if (dbjob == null) return BadRequest();

            ViewBag.Categories = await categoryReadRepository.GetActiveCachingCategories();
            ViewBag.Cities = await cityReadRepository.GetActiveCachingCities();
            ViewBag.Experiences = await experienceReadRepository.GetAllAsync(x => x.Status);
            ViewBag.JobTypes = await jobTypeReadRepository.GetAllAsync(x => x.Status);

            return View(dbjob);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id, Job job, int typeId, int catId, int cityId, int expId)
        {
            using var context = new Context();

            if (id == null) return NotFound();
            Job? dbjob = await context.Jobs.Include(x => x.JobDetail).FirstOrDefaultAsync(x => x.Id == id);
            if (dbjob == null) return BadRequest();

            ViewBag.Categories = await categoryReadRepository.GetActiveCachingCategories();
            ViewBag.Cities = await cityReadRepository.GetActiveCachingCities();
            ViewBag.Experiences = await experienceReadRepository.GetAllAsync(x => x.Status);
            ViewBag.JobTypes = await jobTypeReadRepository.GetAllAsync(x => x.Status);


            
            dbjob.JobTypeId = typeId;
            dbjob.CategoryId = catId;
            dbjob.CityId = cityId;
            dbjob.ExperienceId = expId;
            job.CreatedTime = dbjob.CreatedTime;
            job.PremiumDate = dbjob.PremiumDate;
            job.IsPremium = dbjob.IsPremium;
            job.Status = dbjob.Status;
            job.DeadLine = dbjob.DeadLine;

            dbjob.JobName = job.JobName;
            dbjob.JobDetail.RequiredSkills = job.JobDetail.RequiredSkills;
            dbjob.JobDetail.Responsibilities = job.JobDetail.Responsibilities;
            dbjob.JobDetail.JobGraphic = job.JobDetail.JobGraphic;
            dbjob.JobDetail.Salary  = job.JobDetail.Salary;
            dbjob.JobDetail.Email = job.JobDetail.Email;
            dbjob.JobDetail.Link = job.JobDetail.Link;

            context.Jobs.Update(dbjob);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region PremiumJob
        public IActionResult PremiumJob(int? id)
        {
            if (id == null) return NotFound();
            Job? job = jobReadRepository.Get(x => x.Id == id);
            if (job == null) return BadRequest();

            return View(job);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> PremiumJob(int? id,DateTime date)
        {
            if (id == null) return NotFound();
            Job? job = jobReadRepository.Get(x => x.Id == id);
            if (job == null) return BadRequest();

            if(date.Day <= DateTime.Today.Day)
            {
                ModelState.AddModelError("date", "Düzgün tarixi seçin");
                return View();
            }

            await jobWriteRepository.AddPremiumDate(job, date);
            return RedirectToAction("Index");
        }
        #endregion

        #region PremiumDelete
        public IActionResult PremiumDelete(int? id)
        {
            if (id == null) return NotFound();
            Job? job = jobReadRepository.Get(x => x.Id == id);
            if (job == null) return BadRequest();

            jobWriteRepository.PremiumDelete(job);
            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public IActionResult Activity(int? id)
        {
            if (id == null) return NotFound();
            Job? job = jobReadRepository.Get(x => x.Id == id);
            if (job == null) return BadRequest();

            jobWriteRepository.Activity(job);
            return RedirectToAction("Index");
        }
        #endregion

        #region DeleteFnished
        public async Task<IActionResult> DeleteFinishedDeadline()
        {
            await jobWriteRepository.DeleteFinishedDeadlineAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            Job? job = jobReadRepository.Get(x => x.Id == id);
            if (job == null) return BadRequest();

            jobWriteRepository.Delete(job);
            return RedirectToAction("Index");
        }
        #endregion
    }
}
