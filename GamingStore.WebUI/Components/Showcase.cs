using GamingStore.BLL.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace GamingStore.WebUI.Components
{
    public class Showcase : ViewComponent
    {
        private readonly IProductService _productService;

        public Showcase(IProductService productService)
        {
            _productService = productService;
        }

        public IViewComponentResult Invoke()
        {
            var products = _productService.GetShowcaseProducts(false);
            return View(products);
        }
    }
}
