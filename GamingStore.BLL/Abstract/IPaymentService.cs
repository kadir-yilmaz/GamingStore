using GamingStore.EL.Models;
using Iyzipay.Model;
using System.Threading.Tasks;

namespace GamingStore.BLL.Abstract
{
    public interface IPaymentService
    {
        Task<Payment> MakePayment(
            Order order,
            EL.Models.Address address,
            string cardName,
            string cardNumber,
            string expireMonth,
            string expireYear,
            string cvc
        );
    }
}
