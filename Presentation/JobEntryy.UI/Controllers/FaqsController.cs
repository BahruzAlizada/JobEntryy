using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JobEntryy.UI.Controllers
{
    public class FaqsController : Controller
    {
        private readonly IFaqReadRepository faqReadRepository;
        public FaqsController(IFaqReadRepository faqReadRepository)
        {
            this.faqReadRepository = faqReadRepository;
        }

        #region Index
        public IActionResult Index()
        {
            List<Faq> faqs = faqReadRepository.GetAll();
            return View(faqs);
        }
        #endregion
    }
}
