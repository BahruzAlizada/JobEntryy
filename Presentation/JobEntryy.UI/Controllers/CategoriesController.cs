using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JobEntryy.UI.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryReadRepository categoryReadRepository;
        public CategoriesController(ICategoryReadRepository categoryReadRepository)
        {
            this.categoryReadRepository = categoryReadRepository;
        }

        public async Task<IActionResult> Index()
        {
            List<Category> categories = await categoryReadRepository.GetActiveCachingCategories();
            return View(categories);
        }
    }
}
