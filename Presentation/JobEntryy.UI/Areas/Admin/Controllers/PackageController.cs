using JobEntryy.Application.Abstract;
using JobEntryy.Application.Extensions;
using JobEntryy.Domain.Entities;
using JobEntryy.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobEntryy.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,JobManager,ContactManager")]
    public class PackageController : Controller
    {
        private readonly IPackageReadRepository packageReadRepository;
        private readonly IPackageWriteRepository packageWriteRepository;
        private readonly UserManager<AppUser> userManager;
        public PackageController(IPackageReadRepository packageReadRepository, IPackageWriteRepository packageWriteRepository,
            UserManager<AppUser> userManager)
        {
            this.packageReadRepository = packageReadRepository;
            this.packageWriteRepository = packageWriteRepository;
            this.userManager = userManager;
        }

        #region Index
        public IActionResult Index()
        {
            List<Package> packages = packageReadRepository.GetAll();
            return View(packages);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(Package package)
        {
            packageWriteRepository.Add(package);
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        public IActionResult Update(Guid? id)
        {
            if (id == null) return NotFound();
            Package dbPack = packageReadRepository.Get(x => x.Id == id);
            if (dbPack == null) return BadRequest();

            return View(dbPack);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(Guid? id, Package pack)
        {
            if (id == null) return NotFound();
            Package dbPack = packageReadRepository.Get(x => x.Id == id);
            if (dbPack == null) return BadRequest();

            dbPack.Name = pack.Name;
            dbPack.Description = pack.Description;
            dbPack.Price = pack.Price;
            dbPack.ForCompany = pack.ForCompany;

            dbPack.Updated = DateTime.UtcNow.AddHours(4);
            dbPack.ByChanged = await userManager.FindUserUsernameAsync(User.Identity.Name);

            packageWriteRepository.Update(dbPack);
            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public IActionResult Activity(Guid? id)
        {
            if (id == null) return NotFound();
            Package pack = packageReadRepository.Get(x => x.Id == id);
            if (pack == null) return BadRequest();

            packageWriteRepository.Activity(pack);
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public IActionResult Delete(Guid? id)
        {
            if (id == null) return NotFound();
            Package pack = packageReadRepository.Get(x => x.Id == id);
            if (pack == null) return BadRequest();
            
            packageWriteRepository.Delete(pack);
            return RedirectToAction("Index");
        }
        #endregion
    }
}
