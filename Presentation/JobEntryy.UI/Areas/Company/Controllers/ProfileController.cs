using JobEntryy.Domain.Identity;
using JobEntryy.Persistence.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobEntryy.UI.Areas.Company.Controllers
{
    [Area("Company")]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        public ProfileController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        #region Index
        public async Task<IActionResult> Index()
        {
            using var context = new Context();

            AppUser? user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return BadRequest();

            Domain.Identity.Company? company = await context.Companies.FirstOrDefaultAsync(x => x.Id == user.Id);
            if(company == null) return BadRequest();

            return View(company);
        }
        #endregion
    }
}
