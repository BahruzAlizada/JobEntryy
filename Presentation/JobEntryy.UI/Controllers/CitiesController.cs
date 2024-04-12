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
        public async Task<IActionResult> Index(int page = 1)
        {
            double take = 18;
            ViewBag.PageCount = await cityReadRepository.GetPagedCountAsync(take,x=>x.Status);
            ViewBag.CurrentPage = page;

            List<City> cities = await cityReadRepository.GetPagedListAsync((int)take, page, x => x.Status,orderBy:x=>x.Name);
            return View(cities);
        }
        #endregion
    }
}
