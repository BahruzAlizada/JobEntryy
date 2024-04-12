using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JobEntryy.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
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
        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            SocialMedia dbSocialMedia = socialMediaReadRepository.Get(x => x.Id == id);
            if (dbSocialMedia == null) return BadRequest();

            return View(dbSocialMedia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Update(int? id, SocialMedia socialMedia)
        {
            if (id == null) return NotFound();
            SocialMedia dbSocialMedia = socialMediaReadRepository.Get(x => x.Id == id);
            if (dbSocialMedia == null) return BadRequest();

            dbSocialMedia.Id = socialMedia.Id;
            dbSocialMedia.Status = socialMedia.Status;
            dbSocialMedia.Icon = socialMedia.Icon;
            dbSocialMedia.Link = socialMedia.Link;

            socialMediaWriteRepository.Update(socialMedia);
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            SocialMedia socialMedia = socialMediaReadRepository.Get(x => x.Id == id);
            if (socialMedia == null) return BadRequest();

            socialMediaWriteRepository.Delete(socialMedia);
            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public IActionResult Activity(int? id)
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
