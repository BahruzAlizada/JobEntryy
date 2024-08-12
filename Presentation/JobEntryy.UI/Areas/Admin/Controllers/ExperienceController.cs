using JobEntryy.Application.Abstract;
using JobEntryy.Application.Extensions;
using JobEntryy.Domain.Entities;
using JobEntryy.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Text;

namespace JobEntryy.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,JobManager,ContactManager")]
    public class ExperienceController : Controller
    {
        private readonly IExperienceReadRepository experienceReadRepository;
        private readonly IExperienceWriteRepository experienceWriteRepository;
        private readonly UserManager<AppUser> userManager;
        public ExperienceController(IExperienceReadRepository experienceReadRepository, IExperienceWriteRepository experienceWriteRepository,
           UserManager<AppUser> userManager)
        {
            this.experienceReadRepository = experienceReadRepository;
            this.experienceWriteRepository = experienceWriteRepository;
            this.userManager = userManager;
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
        public IActionResult Update(Guid? id)
        {
            if (id == null) return NotFound();
            Experience dbExp = experienceReadRepository.Get(x=>x.Id==id);
            if (dbExp == null) return BadRequest();

            return View(dbExp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(Guid? id,Experience exp)
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

            dbExp.Name = exp.Name;
            dbExp.Updated = DateTime.UtcNow.AddHours(4);
            dbExp.ByChanged = await userManager.FindUserUsernameAsync(User.Identity.Name);

            experienceWriteRepository.Update(dbExp);
            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public IActionResult Activity(Guid? id)
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
