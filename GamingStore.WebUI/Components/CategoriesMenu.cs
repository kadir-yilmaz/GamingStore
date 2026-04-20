using GamingStore.BLL.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace GamingStore.WebUI.Components
{
    public class CategoriesMenu : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CategoriesMenu(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _categoryService.GetAllCategories(false);
            return View(categories);
        }
    }
}
