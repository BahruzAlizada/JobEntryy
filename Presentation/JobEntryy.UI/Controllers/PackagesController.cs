using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;

namespace JobEntryy.UI.Controllers
{
    public class PackagesController : Controller
    {
        private readonly IPackageReadRepository packageReadRepository;
        private readonly IPackageWriteRepository packageWriteRepository;
        public PackagesController(IPackageReadRepository packageReadRepository, IPackageWriteRepository packageWriteRepository)
        {
            this.packageReadRepository = packageReadRepository;
            this.packageWriteRepository = packageWriteRepository;
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
