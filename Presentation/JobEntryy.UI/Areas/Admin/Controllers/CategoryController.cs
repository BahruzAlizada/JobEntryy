using JobEntryy.Application.Abstract;
using JobEntryy.Application.Abstract.Categories;
using JobEntryy.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobEntryy.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,JobManager,ContactManager")]
    public class CategoryController : Controller
    {
        private readonly ICategoryReadRepository categoryReadRepository;
        private readonly ICategoryWriteRepository categoryWriteRepository;
        public CategoryController(ICategoryReadRepository categoryReadRepository, ICategoryWriteRepository categoryWriteRepository)
        {
            this.categoryReadRepository = categoryReadRepository;
            this.categoryWriteRepository = categoryWriteRepository;
        }

        #region Index
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await categoryReadRepository.GetAllAsync();
            return View(categories);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(Category category)
        {
            bool result = categoryReadRepository.GetAll().Any(x => x.Name == category.Name);
            if(result)
            {
                ModelState.AddModelError("Name", "Bu adda kateqoriya zatən mövcuddur");
                return View();
            }

            categoryWriteRepository.Add(category);
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            Category? dbCategory = categoryReadRepository.Get(x => x.Id == id);
            if(dbCategory == null) return BadRequest();

            return View(dbCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Update(int? id,Category category)
        {
            if (id == null) return NotFound();
            Category? dbCategory = categoryReadRepository.Get(x => x.Id == id);
            if (dbCategory == null) return BadRequest();

            #region Result
            bool result = categoryReadRepository.GetAll().Any(x => x.Name == category.Name && x.Id !=id);
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda kateqoriya zatən mövcuddur");
                return View();
            }
            #endregion

            dbCategory.Name = category.Name;

            categoryWriteRepository.Update(dbCategory);
            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public IActionResult Activity(int? id)
        {
            if (id == null) return NotFound();
            Category? category = categoryReadRepository.Get(x => x.Id == id);
            if (category == null) return BadRequest();

            categoryWriteRepository.Activity(category);
            return RedirectToAction("Index");
        }
        #endregion
    }
}
