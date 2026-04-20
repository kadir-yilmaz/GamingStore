using GamingStore.BLL.Abstract;
using GamingStore.EL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GamingStore.WebUI.Pages
{
    public class CartModel : PageModel
    {
        private readonly IProductService _productService;
        public Cart Cart { get; set; } // IoC
        public string ReturnUrl { get; set; } = "/";

        public CartModel(IProductService productService, Cart cart)
        {
            _productService = productService;
            Cart = cart;
        }

        public void OnGet(string returnUrl)
        {
            Console.WriteLine("ONGET ÇALIŞTI");
            Console.WriteLine(ReturnUrl);
            ReturnUrl = returnUrl ?? "/";
        }

        public IActionResult OnPost(int productId, string returnUrl)
        {
            Console.WriteLine("ONPOST ÇALIŞTI");
            Product? product = _productService
                .GetOneProduct(productId, false);
            if (product is not null)
            {
                Cart.AddItem(product, 1);
            }
            return RedirectToPage(new { returnUrl = returnUrl });
        }

        public IActionResult OnPostRemove(int id, string returnUrl)
        {
            var lineToRemove = Cart.Lines.FirstOrDefault(cl => cl.Product.Id.Equals(id));
            if (lineToRemove != null)
            {
                Cart.RemoveLine(lineToRemove.Product);
            }
            return RedirectToPage(new { returnUrl = returnUrl });
        }

        public IActionResult OnPostIncrease(int id, string returnUrl)
        {
            var line = Cart.Lines.FirstOrDefault(cl => cl.Product.Id == id);
            if (line != null)
            {
                var product = _productService.GetOneProduct(id, false);
                if (product != null && line.Quantity < product.Stock) // stok kontrolü
                {
                    Cart.AddItem(product, 1);
                }
            }
            return RedirectToPage(new { returnUrl = returnUrl });
        }

        public IActionResult OnPostDecrease(int id, string returnUrl)
        {
            var line = Cart.Lines.FirstOrDefault(cl => cl.Product.Id == id);
            if (line != null)
            {
                if (line.Quantity > 1)
                {
                    // Önce ürünü sepetten tamamen çıkar
                    Cart.RemoveLine(line.Product);
                    // Sonra 1 eksik miktarla tekrar ekle
                    var product = _productService.GetOneProduct(id, false);
                    if (product != null)
                    {
                        Cart.AddItem(product, line.Quantity - 1);
                    }
                }
                else
                {
                    // Miktar 1 ise ürünü sepetten tamamen çıkar
                    Cart.RemoveLine(line.Product);
                }
            }
            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}