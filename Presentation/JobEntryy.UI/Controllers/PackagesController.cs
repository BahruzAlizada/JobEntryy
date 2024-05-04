using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;

namespace JobEntryy.UI.Controllers
{
    public class PackagesController : Controller
    {
        private readonly IPackageReadRepository packageReadRepository;
        public PackagesController(IPackageReadRepository packageReadRepository)
        {
            this.packageReadRepository = packageReadRepository;
        }

        #region Index
        public IActionResult Index()
        {
            List<Package> packages = packageReadRepository.GetAll(x => x.Status);
            return View(packages);
        }
        #endregion
    }
}
