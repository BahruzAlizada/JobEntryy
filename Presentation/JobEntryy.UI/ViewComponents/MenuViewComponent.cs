using Microsoft.AspNetCore.Mvc;

namespace JobEntryy.UI.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
