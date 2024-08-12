using JobEntryy.Application.Abstract;
using JobEntryy.Application.Extensions;
using JobEntryy.Domain.Entities;
using JobEntryy.Domain.Identity;
using JobEntryy.Persistence.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobEntryy.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,JobManager,ContactManager")]
    public class JobTypeController : Controller
    {
        private readonly IJobTypeReadRepository jobTypeReadRepository;
        private readonly IJobTypeWriteRepository jobTypeWriteRepository;
        private readonly UserManager<AppUser> userManager;
        public JobTypeController(IJobTypeReadRepository jobTypeReadRepository, IJobTypeWriteRepository jobTypeWriteRepository,
            UserManager<AppUser> userManager)
        {
            this.jobTypeReadRepository = jobTypeReadRepository;
            this.jobTypeWriteRepository = jobTypeWriteRepository;
            this.userManager = userManager;
        }

        #region Index
        public IActionResult Index()
        {
            List<JobType> jobTypes = jobTypeReadRepository.GetAll();
            return View(jobTypes);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(JobType JobType)
        {
            bool result = jobTypeReadRepository.GetAll().Any(x => x.Name == JobType.Name);
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda tip zatən mövcuddur");
                return View();
            }

            jobTypeWriteRepository.Add(JobType);
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        public IActionResult Update(Guid? id)
        {
            if (id == null) return NotFound();
            JobType dbJT = jobTypeReadRepository.Get(x => x.Id == id);
            if (dbJT == null) return BadRequest();

            return View(dbJT);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(Guid? id, JobType JobType)
        {
            if (id == null) return NotFound();
            JobType dbJT = jobTypeReadRepository.Get(x => x.Id == id);
            if (dbJT == null) return BadRequest();

            bool result = jobTypeReadRepository.GetAll().Any(x => x.Name == JobType.Name && x.Id != id);
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda tip zatən mövcuddur");
                return View();
            }

            dbJT.Name = JobType.Name;
            dbJT.Updated = DateTime.UtcNow.AddHours(4);
            dbJT.ByChanged = await userManager.FindUserUsernameAsync(User.Identity.Name);

            jobTypeWriteRepository.Update(dbJT);
            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public IActionResult Activity(Guid? id)
        {
            if (id == null) return NotFound();
            JobType jobType = jobTypeReadRepository.Get(x => x.Id == id);
            if (jobType == null) return BadRequest();

            jobTypeWriteRepository.Activity(jobType);
            return RedirectToAction("Index");
        }
        #endregion
    }
}
