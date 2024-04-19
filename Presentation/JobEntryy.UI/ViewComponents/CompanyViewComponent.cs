using JobEntryy.Application.Abstract;
using JobEntryy.Application.ViewModels;
using JobEntryy.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobEntryy.UI.ViewComponents
{
    public class CompanyViewComponent : ViewComponent
    {
        private readonly ICompanyReadRepository companyReadRepository;
        public CompanyViewComponent(ICompanyReadRepository companyReadRepository)
        {
            this.companyReadRepository = companyReadRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(string username)
        {
            CompanyVM company = await companyReadRepository.GetCompanyAsync(username);
            return View(company);
        }
    }
}
