using Microsoft.AspNetCore.Mvc;

namespace JobEntryy.UI.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
