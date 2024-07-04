using JobEntryy.Application.Abstract;
using JobEntryy.Application.ViewModels;
using JobEntryy.Domain.Identity;
using JobEntryy.Infrastructure.Abstract;
using JobEntryy.Infrastructure.Concrete;
using JobEntryy.Persistence.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace JobEntryy.UI.Areas.Company.Controllers
{
    [Area("Company")]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IJobReadRepository jobReadRepository;
        private readonly IPhotoService photoService;
        private readonly IWebHostEnvironment env;
        public ProfileController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IJobReadRepository jobReadRepository, IPhotoService photoService, IWebHostEnvironment env)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jobReadRepository = jobReadRepository;
            this.photoService = photoService;
            this.env = env;

        }

        #region Index
        public async Task<IActionResult> Index()
        {

            AppUser? user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return BadRequest();

            CompanyVM dbVm = new CompanyVM
            {
                Id = user.Id,
                Image = user.Image,
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email,
                CompanyDescription = user.CompanyDescription,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                WebUrl = user.WebUrl,
                IsPremium = user.IsPremium,
                JobsCount = await jobReadRepository.CompanyJobCountAsync(user.Id),
            };

            return View(dbVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Index(CompanyVM vm)
        {
            AppUser? user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return BadRequest();

            CompanyVM dbVm = new CompanyVM
            {
                Id = user.Id,
                Image = user.Image,
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email,
                CompanyDescription = user.CompanyDescription,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                WebUrl = user.WebUrl,
                IsPremium = user.IsPremium,
                JobsCount = await jobReadRepository.CompanyJobCountAsync(user.Id),
            };


            user.Name = vm.Name;
            user.Email = vm.Email;
            user.CompanyDescription = vm.CompanyDescription;
            user.Address = vm.Address;
            user.PhoneNumber = vm.PhoneNumber;
            user.WebUrl = vm.WebUrl;

            await userManager.UpdateAsync(user);
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "default" });
        }
        #endregion

        #region UploadImage
        public async Task<IActionResult> UploadImage()
        {
            AppUser? user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return BadRequest();

            CompanyVM dbvm = new CompanyVM
            {
                Image = user.Image
            };

            return View(dbvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> UploadImage(CompanyVM company)
        {
            AppUser? user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return BadRequest();

            CompanyVM dbvm = new CompanyVM { Image = user.Image };

           
            #region Image
            (bool isValid, string errorMessage) = await photoService.PhotoChechkValidatorAsync(company.Photo, true, false);
            if (!isValid)
            {
                ModelState.AddModelError("Photo", errorMessage);
                return View();
            }
            string folder = Path.Combine(env.WebRootPath, "assets", "img", "company");
            user.Image = await photoService.SavePhotoAsync(company.Photo, folder);

            string path = Path.Combine(env.WebRootPath, folder, dbvm.Image);
            photoService.DeletePhoto(path);
            #endregion

            await userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
        #endregion

        #region ChangePassword
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ChangePassword(ChangePasswordVM changePassword)
        {
            AppUser? user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return BadRequest();

            bool passwordIsValid = await userManager.CheckPasswordAsync(user, changePassword.OldPassword);
            if (passwordIsValid)
            {
                string newPasswordHash = userManager.PasswordHasher.HashPassword(user, changePassword.NewPassword);
                user.PasswordHash = newPasswordHash;
                var result = await userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }
                await signInManager.SignOutAsync();
                return RedirectToAction("Login", "Account", new { area = "default" });
            }
            else
            {
                ModelState.AddModelError("OldPassword", "Köhnə şifrə yanlışdır !");
                return View();
            }
        }
        #endregion
    }
}
