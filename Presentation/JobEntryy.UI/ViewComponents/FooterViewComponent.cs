using Microsoft.AspNetCore.Mvc;

namespace JobEntryy.UI.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
