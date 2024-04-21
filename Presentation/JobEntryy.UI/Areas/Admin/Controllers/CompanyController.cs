using JobEntryy.Application.Abstract;
using JobEntryy.Application.Abstract.Company;
using JobEntryy.Application.ViewModels;
using JobEntryy.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobEntryy.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ICompanyReadRepository companyReadRepository;
        private readonly ICompanyWriteRepository companyWriteRepository;
        public CompanyController(UserManager<AppUser> userManager, ICompanyReadRepository companyReadRepository, ICompanyWriteRepository companyWriteRepository)
        {
            this.userManager = userManager;
            this.companyReadRepository = companyReadRepository;
            this.companyWriteRepository = companyWriteRepository;
        }

        #region Index
        public async Task<IActionResult> Index(string search, int page = 1)
        {
            double take = 15;
            ViewBag.PageCount = await companyReadRepository.GetAllCompanyPageCountAsync(take);
            ViewBag.CurrentPage = page;

            ViewBag.CompanyCount = await companyReadRepository.GetAllCompanyCountAsync();
            List<CompanyVM> companies = await companyReadRepository.GetAllCompaniesWithPageAsync((int)take, page, search);
            return View(companies);
        }
        #endregion

        #region PremiumCompanies
        public async Task<IActionResult> PremiumCompanies(string search, int page = 1)
        {
            double take = 15;
            ViewBag.PageCount = await companyReadRepository.GetPremiumCompanyPageCountAsync(take);
            ViewBag.CurrentPage = page;

            ViewBag.CompanyCount = await companyReadRepository.GetPremiumCountAsync();
            List<CompanyVM> companies = await companyReadRepository.GetPremiumCompaniesWithPageAsync((int)take, page, search);
            return View(companies);
        }
        #endregion

        #region ResetPassword
        public async Task<IActionResult> ResetPassword(int? id)
        {
            if (id == null) return NotFound();
            AppUser? user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return BadRequest();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ResetPassword(int? id, ResetPasswordVM resetPassword)
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

        #region Update
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            AppUser? company = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (company == null) return BadRequest();

            CompanyVM dbVm = new CompanyVM
            {
                Id = company.Id,
                Name = company.Name,
                CompanyDescription = company.CompanyDescription,
                Email = company.Email,
                WebUrl = company.WebUrl,
                PhoneNumber = company.PhoneNumber,
                Address = company.Address,
            };

            return View(dbVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id, CompanyVM vm)
        {
            if (id == null) return NotFound();
            AppUser? company = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (company == null) return BadRequest();

            CompanyVM dbVm = new CompanyVM
            {
                Id = company.Id,
                Name = company.Name,
                CompanyDescription = company.CompanyDescription,
                Email = company.Email,
                WebUrl = company.WebUrl,
                PhoneNumber = company.PhoneNumber,
                Address = company.Address,
            };

            company.Id = vm.Id;
            company.Name = vm.Name;
            company.Email = vm.Email;
            company.CompanyDescription = vm.CompanyDescription;
            company.Address = vm.Address;
            company.PhoneNumber = vm.PhoneNumber;
            company.WebUrl = vm.WebUrl;

            await userManager.UpdateAsync(company);
            return RedirectToAction("Index");
        }
        #endregion

        #region Premium
        public async Task<IActionResult> Premium(int? id)
        {
            if (id == null) return NotFound();
            AppUser? company = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (company == null) return BadRequest();

            await companyWriteRepository.PremiumAsync(company);
            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null) return NotFound();
            AppUser? company = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if(company == null) return BadRequest();

            await companyWriteRepository.ActivityAsync(company);
            return RedirectToAction("Index");
        }
        #endregion
    }
}
