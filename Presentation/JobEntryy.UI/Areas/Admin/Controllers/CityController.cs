using JobEntryy.Application.Abstract;
using JobEntryy.Application.Extensions;
using JobEntryy.Domain.Entities;
using JobEntryy.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace JobEntryy.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,JobManager,ContactManager")]
    public class CityController : Controller
    {
        private readonly ICityReadRepository cityReadRepository;
        private readonly ICityWriteRepository cityWriteRepository;
        private readonly UserManager<AppUser> userManager;
        public CityController(ICityReadRepository cityReadRepository, ICityWriteRepository cityWriteRepository,
            UserManager<AppUser> userManager)
        {
            this.cityReadRepository = cityReadRepository;
            this.cityWriteRepository = cityWriteRepository;
            this.userManager = userManager;
        }

        #region Index
        public async Task<IActionResult> Index(int page = 1)
        {
            double take = 20;
            ViewBag.PageCount = await cityReadRepository.GetPagedCountAsync(take);
            ViewBag.CurrentPage = page;

            List<City> cities = await cityReadRepository.GetPagedListAsync((int)take, page,orderBy:x=>x.Name);
            return View(cities);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(City city)
        {
            bool result = cityReadRepository.GetAll().Any(x => x.Name == city.Name);
            if(result)
            {
                ModelState.AddModelError("Name", "Bu adda şəhər zatən mövcuddur");
                return View();
            }

            cityWriteRepository.Add(city);
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        public IActionResult Update(Guid? id)
        {
            if (id == null) return NotFound();
            City dbCity = cityReadRepository.Get(x => x.Id == id);
            if (dbCity == null) return BadRequest();

            return View(dbCity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(Guid? id, City city)
        {
            if (id == null) return NotFound();
            City dbCity = cityReadRepository.Get(x => x.Id == id);
            if (dbCity == null) return BadRequest();

            bool result = cityReadRepository.GetAll().Any(x => x.Name == city.Name && x.Id!=id);
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda şəhər zatən mövcuddur");
                return View();
            }

            dbCity.Name = city.Name;
            dbCity.Updated = DateTime.UtcNow.AddHours(4);
            dbCity.ByChanged = await userManager.FindUserUsernameAsync(User.Identity.Name);

            cityWriteRepository.Update(dbCity);
            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public IActionResult Activity(Guid? id)
        {
            if (id == null) return NotFound();
            City city = cityReadRepository.Get(x => x.Id == id);
            if (city == null) return BadRequest();

            cityWriteRepository.Activity(city);
            return RedirectToAction("Index");
        }
        #endregion
    }
}
