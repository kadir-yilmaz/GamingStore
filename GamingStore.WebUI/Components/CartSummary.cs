using GamingStore.EL.Models;
using Microsoft.AspNetCore.Mvc;

namespace GamingStore.WebUI.Components
{
    public class CartSummary : ViewComponent
    {
        private readonly Cart _cart;
        public CartSummary(Cart cartService)
        {
            _cart = cartService;
        }

        public string Invoke()
        {
            return _cart.ComputeTotalItems().ToString();
        }
    }
}
