using JobEntryy.Domain.Identity;
using JobEntryy.Persistence.Concrete;
using JobEntryy.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobEntryy.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,JobManager,ContactManager")]
    public class DashboardController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        public DashboardController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        #region Index
        public async Task<IActionResult> Index()
        {
            using var context = new Context();

            var statisticsTask = Task.Run(async () => new StatitcsModel
            {
                ActiveJobCount = await context.Jobs.Where(x => x.Status).CountAsync(),
                TodayAddJobCount = await context.Jobs.Where(x => x.CreatedTime.Date == DateTime.Today).CountAsync(),
                ActiveCompanyCount = await userManager.Users.Where(x => x.Status && x.UserRole.Contains("Company")).CountAsync(),
                CategoryCount = await context.Categories.Where(x=>x.Status).CountAsync(),
                CityCount = await context.Cities.Where(x=>x.Status).CountAsync(),
                ContactCount = await context.Contacts.CountAsync()
            });

            await Task.WhenAll(statisticsTask); // Gözləmək üçün tələb olunur

            var model = statisticsTask.Result;
            return View(model);
        }
        #endregion
    }
}
