using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobEntryy.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,JobManager,ContactManager")]
    public class ContactController : Controller
    {
        private readonly IContactReadRepository contactReadRepository;
        private readonly IContactWriteRepository contactWriteRepository;
        public ContactController(IContactReadRepository contactReadRepository, IContactWriteRepository contactWriteRepository)
        {
            this.contactReadRepository = contactReadRepository;
            this.contactWriteRepository = contactWriteRepository;
        }

        #region Index
        public async Task<IActionResult> Index(int page = 1)
        {
            double take = 18;
            ViewBag.PageCount = await contactReadRepository.GetPagedCountAsync(take);
            ViewBag.CurrentPage = page;

            List<Contact> contacts = await contactReadRepository.GetPagedListAsync((int)take, page, orderByDescending:x=>x.Id);
            return View(contacts);
        }
        #endregion

        #region Detail
        public IActionResult Detail(Guid? id)
        {
            if (id == null) return NotFound();
            Contact contact = contactReadRepository.Get(x => x.Id == id);
            if (contact == null) return BadRequest();

            return View(contact);
        }
        #endregion

        #region Delete
        public IActionResult Delete(Guid? id)
        {
            if (id == null) return NotFound();
            Contact contact = contactReadRepository.Get(x => x.Id == id);
            if (contact == null) return BadRequest();

            contactWriteRepository.Delete(contact);
            return RedirectToAction("Index");
        }
        #endregion
    }
}
