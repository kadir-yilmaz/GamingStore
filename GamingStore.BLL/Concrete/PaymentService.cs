using GamingStore.BLL.Abstract;
using GamingStore.EL.Models;
using GamingStore.EL.Settings;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Linq;

namespace GamingStore.BLL.Concrete
{
    public class PaymentService : IPaymentService
    {
        private readonly Iyzipay.Options _options;

        public PaymentService(IOptions<IyzipaySettings> settings)
        {
            var s = settings.Value;
            _options = new Iyzipay.Options
            {
                ApiKey = s.ApiKey,
                SecretKey = s.SecretKey,
                BaseUrl = s.BaseUrl
            };
        }

        public async Task<Payment> MakePayment(Order order, EL.Models.Address address, string cardName, string cardNumber, string expireMonth, string expireYear, string cvc)
        {
            var request = new CreatePaymentRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = $"GamingStore-{order.Id}",
                Price = order.Lines.Sum(l => l.Product.Price * l.Quantity).ToString("F2", CultureInfo.InvariantCulture),
                PaidPrice = order.Lines.Sum(l => l.Product.Price * l.Quantity).ToString("F2", CultureInfo.InvariantCulture),
                Currency = Currency.TRY.ToString(),
                Installment = 1,
                BasketId = order.Id.ToString(),
                PaymentChannel = PaymentChannel.WEB.ToString(),
                PaymentGroup = PaymentGroup.PRODUCT.ToString(),
                PaymentCard = new PaymentCard
                {
                    CardHolderName = cardName,
                    CardNumber = cardNumber,
                    ExpireMonth = expireMonth,
                    ExpireYear = expireYear,
                    Cvc = cvc,
                    RegisterCard = 0
                },
                Buyer = new Buyer
                {
                    Id = "BY" + order.Id,
                    Name = address.FirstName,
                    Surname = address.LastName,
                    GsmNumber = address.PhoneNumber,
                    Email = "test@email.com",
                    IdentityNumber = "74300864791",
                    RegistrationAddress = address.AddressDetail,
                    City = address.Province,
                    Country = "Turkey",
                    ZipCode = address.PostalCode,
                    Ip = "127.0.0.1"
                },
                ShippingAddress = new Iyzipay.Model.Address
                {
                    ContactName = address.FirstName + " " + address.LastName,
                    City = address.Province,
                    Country = "Turkey",
                    Description = address.AddressDetail,
                    ZipCode = address.PostalCode
                },
                BillingAddress = new Iyzipay.Model.Address
                {
                    ContactName = address.FirstName + " " + address.LastName,
                    City = address.Province,
                    Country = "Turkey",
                    Description = address.AddressDetail,
                    ZipCode = address.PostalCode
                },
                BasketItems = order.Lines.Select((line, i) => new Iyzipay.Model.BasketItem
                {
                    Id = "BI" + (i + 1),
                    Name = line.Product.Name,
                    Category1 = "Category",
                    ItemType = BasketItemType.PHYSICAL.ToString(),
                    Price = (line.Product.Price * line.Quantity).ToString("F2", CultureInfo.InvariantCulture)
                }).ToList()
            };

            return await Payment.Create(request, _options);
        }
    }
}
