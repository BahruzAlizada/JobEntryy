using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace JobEntryy.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CityController : Controller
    {
        private readonly ICityReadRepository cityReadRepository;
        private readonly ICityWriteRepository cityWriteRepository;
        public CityController(ICityReadRepository cityReadRepository, ICityWriteRepository cityWriteRepository)
        {
            this.cityReadRepository = cityReadRepository;
            this.cityWriteRepository = cityWriteRepository;
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
        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            City dbCity = cityReadRepository.Get(x => x.Id == id);
            if (dbCity == null) return BadRequest();

            return View(dbCity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Update(int? id, City city)
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

            dbCity.Id = city.Id;
            dbCity.Name = city.Name;
            dbCity.Status = city.Status;

            cityWriteRepository.Update(city);
            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public IActionResult Activity(int? id)
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
