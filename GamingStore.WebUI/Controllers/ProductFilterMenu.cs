using Microsoft.AspNetCore.Mvc;

namespace GamingStore.WebUI.Controllers
{
    public class ProductFilterMenu : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
