using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JobEntryy.UI.Controllers
{
    public class CitiesController : Controller
    {
        private readonly ICityReadRepository cityReadRepository;
        public CitiesController(ICityReadRepository cityReadRepository)
        {
            this.cityReadRepository = cityReadRepository;
        }

        #region Index
        public async Task<IActionResult> Index(string search,int page = 1)
        {
            ViewBag.PageCount = await cityReadRepository.GetPagedCountAsync(take:18,x=>x.Status);
            ViewBag.CurrentPage = page;

            List<City> cities = await cityReadRepository.GetCitiesWithPagedAsync(take:18, page, search);
            return View(cities);
        }
        #endregion
    }
}
