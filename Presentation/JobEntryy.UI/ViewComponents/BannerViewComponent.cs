using Microsoft.AspNetCore.Mvc;

namespace JobEntryy.UI.ViewComponents
{
    public class BannerViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
