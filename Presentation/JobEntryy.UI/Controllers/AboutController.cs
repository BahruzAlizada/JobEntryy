using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JobEntryy.UI.Controllers
{
    public class AboutController : Controller
    {
        private readonly IAboutReadRepository aboutReadRepository;
        public AboutController(IAboutReadRepository aboutReadRepository)
        {
            this.aboutReadRepository = aboutReadRepository;
        }
        public IActionResult Index()
        {
            About about = aboutReadRepository.Get();
            return View(about);
        }
    }
}
