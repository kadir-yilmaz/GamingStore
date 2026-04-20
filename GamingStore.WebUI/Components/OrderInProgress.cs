using GamingStore.BLL.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace GamingStore.WebUI.Components
{
    public class OrderInProgress : ViewComponent
    {
        private readonly IOrderService _orderService;

        public OrderInProgress(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public string Invoke()
        {
            return _orderService
                .NumberOfInProcess
                .ToString();
        }
    }
}
