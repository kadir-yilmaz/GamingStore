using GamingStore.BLL.Abstract;
using GamingStore.EL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingStore.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Editor")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            var orders = _orderService.Orders.ToList();
            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int orderId, OrderStatus status)
        {
            await _orderService.UpdateStatusAsync(orderId, status);
            TempData["Success"] = "Sipariş durumu güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Ship(int orderId, string cargoCompany, string trackingNumber)
        {
            await _orderService.ShipOrderAsync(orderId, cargoCompany, trackingNumber);
            TempData["Success"] = $"Sipariş kargoya verildi. {cargoCompany} - {trackingNumber}";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.CancelOrderAsync(id);
            TempData["Success"] = "Sipariş iptal edildi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Complete([FromForm] int id)
        {
            await _orderService.CompleteAsync(id);
            TempData["Success"] = "Sipariş tamamlandı.";
            return RedirectToAction("Index");
        }
    }
}
