using JobEntryy.Application.Abstract;
using JobEntryy.Application.Extensions;
using JobEntryy.Domain.Entities;
using JobEntryy.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobEntryy.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,JobManager,ContactManager")]
    public class FaqController : Controller
    {
        private readonly IFaqReadRepository faqReadRepository;
        private readonly IFaqWriteRepository faqWriteRepository;
        private readonly UserManager<AppUser> userManager;
        public FaqController(IFaqReadRepository faqReadRepository, IFaqWriteRepository faqWriteRepository, UserManager<AppUser> userManager)
        {
            this.faqReadRepository = faqReadRepository;
            this.faqWriteRepository = faqWriteRepository;
            this.userManager = userManager;
        }

        #region Index
        public IActionResult Index()
        {
            List<Faq> faqs = faqReadRepository.GetAll();
            return View(faqs);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(Faq faq)
        {
            faqWriteRepository.Add(faq);
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        public IActionResult Update(Guid? id)
        {
            if (id == null) return NotFound();
            Faq? dbFaq = faqReadRepository.Get(x => x.Id == id);
            if (dbFaq == null) return BadRequest();

            return View(dbFaq);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(Guid? id, Faq faq)
        {
            if (id == null) return NotFound();
            Faq? dbFaq = faqReadRepository.Get(x => x.Id == id);
            if (dbFaq == null) return BadRequest();

            dbFaq.Answer = faq.Answer;
            dbFaq.Quetsion = faq.Quetsion;
            dbFaq.Updated = DateTime.UtcNow.AddHours(4);
            dbFaq.ByChanged = await userManager.FindUserUsernameAsync(User.Identity.Name);

            faqWriteRepository.Update(dbFaq);
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public IActionResult Delete(Guid? id)
        {
            if (id == null) return NotFound();
            Faq? faq = faqReadRepository.Get(x => x.Id == id);
            if (faq == null) return BadRequest();

            faqWriteRepository.Delete(faq);
            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public IActionResult Activity(Guid? id)
        {
            if (id == null) return NotFound();
            Faq? faq = faqReadRepository.Get(x => x.Id == id);
            if (faq == null) return BadRequest();

            faqWriteRepository.Activity(faq);
            return RedirectToAction("Index");
        }
        #endregion
    }
}

