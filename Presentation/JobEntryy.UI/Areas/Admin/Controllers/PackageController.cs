using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JobEntryy.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PackageController : Controller
    {
        private readonly IPackageReadRepository packageReadRepository;
        private readonly IPackageWriteRepository packageWriteRepository;
        public PackageController(IPackageReadRepository packageReadRepository, IPackageWriteRepository packageWriteRepository)
        {
            this.packageReadRepository = packageReadRepository;
            this.packageWriteRepository = packageWriteRepository;
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
        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            Package dbPack = packageReadRepository.Get(x => x.Id == id);
            if (dbPack == null) return BadRequest();

            return View(dbPack);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Update(int? id, Package pack)
        {
            if (id == null) return NotFound();
            Package dbPack = packageReadRepository.Get(x => x.Id == id);
            if (dbPack == null) return BadRequest();

            dbPack.Id = pack.Id;
            dbPack.Status = pack.Status;
            dbPack.Name = pack.Name;
            dbPack.Description = pack.Description;
            dbPack.Price = pack.Price;
            dbPack.ForCompany = pack.ForCompany;

            packageWriteRepository.Update(pack);
            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public IActionResult Activity(int? id)
        {
            if (id == null) return NotFound();
            Package pack = packageReadRepository.Get(x => x.Id == id);
            if (pack == null) return BadRequest();

            packageWriteRepository.Activity(pack);
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public IActionResult Delete(int? id)
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
