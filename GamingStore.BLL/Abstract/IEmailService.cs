using GamingStore.EL.Models;

namespace GamingStore.BLL.Abstract
{
    public interface IEmailService
    {
        Task SendOrderConfirmationAsync(string customerEmail, Order order);
    }
}
