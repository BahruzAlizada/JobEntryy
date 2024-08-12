using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobEntryy.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,JobManager,ContactManager")]
    public class SocialMediaController : Controller
    {
        private readonly ISocialMediaReadRepository socialMediaReadRepository;
        private readonly ISocialMediaWriteRepository socialMediaWriteRepository;
        public SocialMediaController(ISocialMediaReadRepository socialMediaReadRepository, ISocialMediaWriteRepository socialMediaWriteRepository)
        {
            this.socialMediaReadRepository = socialMediaReadRepository;
            this.socialMediaWriteRepository = socialMediaWriteRepository;
        }

        #region Index
        public IActionResult Index()
        {
            List<SocialMedia> socialMedias = socialMediaReadRepository.GetAll();
            return View(socialMedias);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(SocialMedia socialMedia)
        {
            socialMediaWriteRepository.Add(socialMedia);
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        public IActionResult Update(Guid? id)
        {
            if (id == null) return NotFound();
            SocialMedia dbSocialMedia = socialMediaReadRepository.Get(x => x.Id == id);
            if (dbSocialMedia == null) return BadRequest();

            return View(dbSocialMedia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Update(Guid? id, SocialMedia socialMedia)
        {
            if (id == null) return NotFound();
            SocialMedia dbSocialMedia = socialMediaReadRepository.Get(x => x.Id == id);
            if (dbSocialMedia == null) return BadRequest();

            dbSocialMedia.Icon = socialMedia.Icon;
            dbSocialMedia.Link = socialMedia.Link;

            socialMediaWriteRepository.Update(dbSocialMedia);
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public IActionResult Delete(Guid? id)
        {
            if (id == null) return NotFound();
            SocialMedia socialMedia = socialMediaReadRepository.Get(x => x.Id == id);
            if (socialMedia == null) return BadRequest();

            socialMediaWriteRepository.Delete(socialMedia);
            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public IActionResult Activity(Guid? id)
        {
            if (id == null) return NotFound();
            SocialMedia socialMedia = socialMediaReadRepository.Get(x => x.Id == id);
            if (socialMedia == null) return BadRequest();

            socialMediaWriteRepository.Activity(socialMedia);
            return RedirectToAction("Index");
        }
        #endregion
    }
}
