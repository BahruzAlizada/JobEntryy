using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JobEntryy.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FaqController : Controller
    {
        private readonly IFaqReadRepository faqReadRepository;
        private readonly IFaqWriteRepository faqWriteRepository;
        public FaqController(IFaqReadRepository faqReadRepository, IFaqWriteRepository faqWriteRepository)
        {
            this.faqReadRepository = faqReadRepository;
            this.faqWriteRepository = faqWriteRepository;
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
        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            Faq? dbFaq = faqReadRepository.Get(x => x.Id == id);
            if (dbFaq == null) return BadRequest();

            return View(dbFaq);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Update(int? id, Faq faq)
        {
            if (id == null) return NotFound();
            Faq? dbFaq = faqReadRepository.Get(x => x.Id == id);
            if (dbFaq == null) return BadRequest();

            dbFaq.Id = faq.Id;
            dbFaq.Status = faq.Status;
            dbFaq.Answer = faq.Answer;
            dbFaq.Quetsion = faq.Quetsion;

            faqWriteRepository.Update(faq);
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            Faq? faq = faqReadRepository.Get(x => x.Id == id);
            if (faq == null) return BadRequest();

            faqWriteRepository.Delete(faq);
            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public IActionResult Activity(int? id)
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

