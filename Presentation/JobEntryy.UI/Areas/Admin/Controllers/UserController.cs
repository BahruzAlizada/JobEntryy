using JobEntryy.Application.Abstract;
using JobEntryy.Application.Abstract.Company;
using JobEntryy.Application.ViewModels;
using JobEntryy.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace JobEntryy.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;
        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        #region Index
        public async Task<IActionResult> Index()
        {
            List<AppUser> users = await userManager.Users.Where(x => !x.UserRole.Contains("Company")).ToListAsync();
            List<UserVM> userVM = new List<UserVM>();

            foreach (AppUser item in users)
            {
                UserVM vm = new UserVM
                {
                    Id = item.Id,
                    Name = item.Name,
                    Email = item.Email,
                    Username = item.UserName,
                    Status = item.Status,
                    Created = item.Created,
                    Role = (await userManager.GetRolesAsync(item))[0]
                };
                userVM.Add(vm);
            }
            return View(userVM);
        }
        #endregion

        #region Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = await roleManager.Roles.Where(x => !x.Name.Contains("Company")).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(UserVM vm, Guid roleId)
        {
            ViewBag.Roles = await roleManager.Roles.Where(x => !x.Name.Contains("Company")).ToListAsync();
            AppRole? role = await roleManager.Roles.FirstOrDefaultAsync(x => x.Id == roleId);
            if (role == null) return BadRequest();

            AppUser user = new AppUser
            {
                Id = vm.Id,
                Name = vm.Name,
                UserName = Guid.NewGuid().ToString("N").Substring(0, 8),
                Email = vm.Email,
                Status = true
            };

            IdentityResult result = await userManager.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            await userManager.AddToRoleAsync(user, role.Name);
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(Guid? id)
        {
            if (id == null) return NotFound();
            AppUser? user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return BadRequest();

            UserVM vm = new UserVM
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(Guid? id, UserVM newVM)
        {
            #region Get
            if (id == null) return NotFound();
            AppUser? user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return BadRequest();

            UserVM vm = new UserVM
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
            };
            #endregion

            user.Id = newVM.Id;
            user.Name = newVM.Name;
            user.Email = newVM.Email;

            await userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
        #endregion

        #region ResetPassword
        public async Task<IActionResult> ResetPassword(Guid? id)
        {
            if (id == null) return NotFound();
            AppUser? user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return BadRequest();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ResetPassword(Guid? id, ResetPasswordVM resetPassword)
        {
            if (id == null) return NotFound();
            AppUser? user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return BadRequest();

            string token = await userManager.GeneratePasswordResetTokenAsync(user);

            IdentityResult result = await userManager.ResetPasswordAsync(user, token, resetPassword.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region RoleChange
        public async Task<IActionResult> RoleChange(Guid? id)
        {
            if (id == null) return NotFound();
            AppUser? user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return BadRequest();

            ViewBag.Roles = await roleManager.Roles.Where(x => !x.Name.Contains("Company")).Select(x => x.Name).ToListAsync();

            RoleVM roleVM = new RoleVM
            {
                Role = (await userManager.GetRolesAsync(user))[0]
            };

            return View(roleVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> RoleChange(Guid? id, string role)
        {
            if (id == null) return NotFound();
            AppUser? user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return BadRequest();

            ViewBag.Roles = await roleManager.Roles.Where(x => !x.Name.Contains("Company")).Select(x => x.Name).ToListAsync();

            RoleVM roleVM = new RoleVM
            {
                Role = (await userManager.GetRolesAsync(user))[0]
            };

            await userManager.RemoveFromRoleAsync(user, roleVM.Role);
            await userManager.AddToRoleAsync(user, role);

            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public async Task<IActionResult> Activity(string username)
        {
            if (username == null) return NotFound();
            AppUser? user = await userManager.FindByNameAsync(username);
            if(user == null) return BadRequest();

            if (user.Status)
                user.Status = false;
            else
                user.Status = true;

            await userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(string username)
        {
            if (username == null) return NotFound();
            AppUser? user = await userManager.FindByNameAsync(username);
            if (user == null) return BadRequest();

            await userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }
        #endregion
    }
}
