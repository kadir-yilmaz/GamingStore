using GamingStore.BLL.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace GamingStore.WebUI.Components
{
    public class CategorySummary : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CategorySummary(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public string Invoke()
        {
            return _categoryService
                .GetAllCategories(false)
                .Count()
                .ToString();
        }
    }
}
