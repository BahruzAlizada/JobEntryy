using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobEntryy.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="SuperAdmin,Admin,JobManager,ContactManager")]
    public class AboutController : Controller
    {
        private readonly IAboutReadRepository aboutReadRepository;
        private readonly IAboutWriteRepository aboutWriteRepository;
        public AboutController(IAboutReadRepository aboutReadRepository, IAboutWriteRepository aboutWriteRepository)
        {
            this.aboutReadRepository = aboutReadRepository;
            this.aboutWriteRepository = aboutWriteRepository;
        }

        #region Index
        public IActionResult Index()
        {
            About about = aboutReadRepository.Get();
            return View(about);
        }
        #endregion

        #region Update
        public IActionResult Update(Guid? id)
        {
            if (id == null) return NotFound();
            About dbAbout = aboutReadRepository.Get(x => x.Id == id);
            if (dbAbout == null) return BadRequest();

            return View(dbAbout);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Update(Guid? id, About about)
        {
            if (id == null) return NotFound();
            About dbAbout = aboutReadRepository.Get(x => x.Id == id);
            if (dbAbout == null) return BadRequest();

            dbAbout.Description = about.Description;

            aboutWriteRepository.Update(dbAbout);
            return RedirectToAction("Index");
        }
        #endregion
    }
}
