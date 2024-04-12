using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using JobEntryy.Persistence.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace JobEntryy.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class JobTypeController : Controller
    {
        private readonly IJobTypeReadRepository jobTypeReadRepository;
        private readonly IJobTypeWriteRepository jobTypeWriteRepository;
        public JobTypeController(IJobTypeReadRepository jobTypeReadRepository, IJobTypeWriteRepository jobTypeWriteRepository)
        {
            this.jobTypeReadRepository = jobTypeReadRepository;
            this.jobTypeWriteRepository = jobTypeWriteRepository;
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
        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            JobType dbJT = jobTypeReadRepository.Get(x => x.Id == id);
            if (dbJT == null) return BadRequest();

            return View(dbJT);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Update(int? id, JobType JobType)
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

            dbJT.Id = JobType.Id;
            dbJT.Status = JobType.Status;
            dbJT.Name = JobType.Name;

            jobTypeWriteRepository.Update(JobType);
            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public IActionResult Activity(int? id)
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
