using JobEntryy.Domain.Identity;
using JobEntryy.Persistence.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace JobEntryy.UI.Controllers
{
    public class CompaniesController : Controller
    {
        public IActionResult Index()
        {
            using var context = new Context();

            List<Company> company = context.Companies.ToList();
            return View(company);
        }
    }
}
