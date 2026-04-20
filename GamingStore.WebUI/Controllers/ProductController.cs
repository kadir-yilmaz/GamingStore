using GamingStore.BLL.Abstract;
using GamingStore.EL.Dtos;
using GamingStore.EL.Models;
using GamingStore.EL.RequestParameters;
using GamingStore.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GamingStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        //public IActionResult Index(ProductRequestParameters p)
        //{
        //    var products = _productService.GetAllProductsWithDetails(p);
        //    var pagination = new Pagination()
        //    {
        //        CurrenPage = p.PageNumber,
        //        ItemsPerPage = p.PageSize,
        //        TotalItems = _productService.GetAllProducts(false).Count()
        //    };
        //    return View(new ProductListViewModel()
        //    {
        //        Products = products,
        //        Pagination = pagination
        //    });
        //}

        public IActionResult Index(ProductRequestParameters p)
        {
            // 1️⃣ Filtrelenmiş ürünlerin toplamını al, pagination için
            var filteredProducts = _productService.GetAllProductsWithDetails(new ProductRequestParameters
            {
                CategoryId = p.CategoryId,
                SearchTerm = p.SearchTerm,
                MinPrice = p.MinPrice,
                MaxPrice = p.MaxPrice,
                PageNumber = 1,
                PageSize = int.MaxValue // sadece sayıyı almak için
            });

            // 2️⃣ Sayfaya özel ürünleri al
            var products = _productService.GetAllProductsWithDetails(p);

            // 3️⃣ Pagination objesini oluştur
            var pagination = new Pagination()
            {
                CurrentPage = p.PageNumber,
                ItemsPerPage = p.PageSize,
                TotalItems = filteredProducts.Count()
            };

            // 4️⃣ ViewModel'i oluştur ve gönder
            return View(new ProductListViewModel()
            {
                Products = products,
                Pagination = pagination
            });
        }

        public IActionResult Get([FromRoute(Name = "id")] int id)
        {
            var model = _productService.GetOneProduct(id, false);
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] ProductDtoForInsertion productDtoForInsertion)
        {
            if (ModelState.IsValid)
            {
                _productService.CreateProduct(productDtoForInsertion);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
