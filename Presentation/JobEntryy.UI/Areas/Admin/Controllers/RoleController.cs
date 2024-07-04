using JobEntryy.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobEntryy.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<AppRole> roleManager;
        public RoleController(RoleManager<AppRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        #region Index
        public async Task<IActionResult> Index()
        {
            List<AppRole> roles = await roleManager.Roles.ToListAsync();
            return View(roles);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(AppRole role)
        {
            role.NormalizedName = role.Name.ToUpper();
            await roleManager.CreateAsync(role);
            return RedirectToAction("Index");
        }
        #endregion
    }
}
