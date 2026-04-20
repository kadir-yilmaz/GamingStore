using GamingStore.DAL;
using Microsoft.AspNetCore.Mvc;

namespace GamingStore.WebUI.Components
{
    public class ProductSummary : ViewComponent
    {
        private readonly AppDbContext _context;

        public ProductSummary(AppDbContext context)
        {
            _context = context;
        }

        public string Invoke()
        {
            return _context.Products.Count().ToString();
        }
    }
}
