using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JobEntryy.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ExperienceController : Controller
    {
        private readonly IExperienceReadRepository experienceReadRepository;
        private readonly IExperienceWriteRepository experienceWriteRepository;
        public ExperienceController(IExperienceReadRepository experienceReadRepository, IExperienceWriteRepository experienceWriteRepository)
        {
            this.experienceReadRepository = experienceReadRepository;
            this.experienceWriteRepository = experienceWriteRepository;
        }

        #region Index
        public IActionResult Index()
        {
            List<Experience> experiences = experienceReadRepository.GetAll(); 
            return View(experiences);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  

        public IActionResult Create(Experience experience)
        {
            bool result = experienceReadRepository.GetAll().Any(x=>x.Name== experience.Name);
            if(result)
            {
                ModelState.AddModelError("Name", "Bu adda təcrübə zatən mövcuddur");
                return View();
            }

            experienceWriteRepository.Add(experience);
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            Experience dbExp = experienceReadRepository.Get(x=>x.Id==id);
            if (dbExp == null) return BadRequest();

            return View(dbExp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Update(int? id,Experience exp)
        {
            if (id == null) return NotFound();
            Experience dbExp = experienceReadRepository.Get(x => x.Id == id);
            if (dbExp == null) return BadRequest();

            bool result = experienceReadRepository.GetAll().Any(x => x.Name == exp.Name && x.Id != id);
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda təcrübə zatən mövcuddur");
                return View();
            }

            dbExp.Id = exp.Id;
            dbExp.Status = exp.Status;
            dbExp.Name = exp.Name;

            experienceWriteRepository.Update(exp);
            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public IActionResult Activity(int? id)
        {
            if (id == null) return NotFound();
            Experience exp = experienceReadRepository.Get(x => x.Id == id);
            if (exp == null) return BadRequest();

            experienceWriteRepository.Activity(exp);
            return RedirectToAction("Index");
        }
        #endregion
    }
}
