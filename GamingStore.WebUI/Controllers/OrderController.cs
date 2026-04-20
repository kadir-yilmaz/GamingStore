using GamingStore.BLL.Abstract;
using Microsoft.EntityFrameworkCore;
using GamingStore.EL.Models;
using GamingStore.EL.Models.GamingStore.EL.Models;
using GamingStore.WebUI.ViewModels;
using GamingStore.WebUI.ViewModels.Location;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GamingStore.WebUI.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IAddressService _addressService;
        private readonly IEmailService _mailService;
        private readonly Cart _cart;
        private readonly IPaymentService _paymentService;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderController(
            IOrderService orderService,
            IAddressService addressService,
            IEmailService mailService,
            Cart cart,
            IPaymentService paymentService,
            UserManager<IdentityUser> userManager)
        {
            _orderService = orderService;
            _addressService = addressService;
            _mailService = mailService;
            _cart = cart;
            _paymentService = paymentService;
            _userManager = userManager;
        }

        private string GetUserId() => _userManager.GetUserId(User)!;

        public IActionResult Index()
        {
            var userId = GetUserId();
            var orders = _orderService.Orders
                .Include(o => o.Lines)
                .ThenInclude(l => l.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderedAt)
                .ToList();

            return View(orders);
        }

        public IActionResult Checkout()
        {
            if (_cart.Lines.Count == 0)
            {
                // Sepet boşsa sepete geri gönder
                return RedirectToAction("Index", "Basket");
            }

            var userId = GetUserId();
            var model = new CheckoutViewModel();

            // Kayıtlı adresleri yükle
            model.DeliveryAddresses = _addressService.GetUserAddresses(userId, AddressType.Delivery).ToList();
            model.InvoiceAddresses = _addressService.GetUserAddresses(userId, AddressType.Invoice).ToList();

            // Varsayılan adresleri seç
            var defaultDelivery = _addressService.GetDefaultAddress(userId, AddressType.Delivery);
            var defaultInvoice = _addressService.GetDefaultAddress(userId, AddressType.Invoice);

            if (defaultDelivery != null)
            {
                model.SelectedDeliveryAddressId = defaultDelivery.Id;
            }
            else if (model.DeliveryAddresses.Any())
            {
                model.SelectedDeliveryAddressId = model.DeliveryAddresses.First().Id;
            }

            if (defaultInvoice != null)
            {
                model.SelectedInvoiceAddressId = defaultInvoice.Id;
            }
            else if (model.InvoiceAddresses.Any())
            {
                model.SelectedInvoiceAddressId = model.InvoiceAddresses.First().Id;
            }

            // Kayıtlı adres yoksa yeni adres girişine yönlendir
            model.UseNewAddress = !model.DeliveryAddresses.Any();

            // JSON dosyasını oku (yeni adres girişi için)
            var jsonString = System.IO.File.ReadAllText("wwwroot/data/il_ilce_mahalle.json");
            var jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<string>>>>(jsonString);
            model.Provinces = jsonData?.Select(c => new Province
            {
                Name = c.Key,
                Districts = c.Value.Select(d => new District
                {
                    Name = d.Key,
                    Neighborhoods = d.Value
                }).OrderBy(d => d.Name).ToList()
            }).OrderBy(p => p.Name).ToList() ?? new List<Province>();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout([FromForm] CheckoutViewModel vm, bool SaveNewAddress = false)
        {
            var userId = GetUserId();

            // Kayıtlı adres mi yoksa yeni adres mi kullanılıyor, ona göre validation temizle
            if (!vm.UseNewAddress)
            {
                // Kayıtlı adres kullanılıyorsa, yeni adres alanlarını doğrulamadan muaf tut
                foreach (var key in ModelState.Keys.Where(k => k.StartsWith("Address.")).ToList())
                {
                    ModelState.Remove(key);
                }
            }
            else
            {
                // Yeni adres kullanılıyorsa, kayıtlı adres seçimini doğrulamadan muaf tut 
                // (SelectedDeliveryAddressId zaten int? olduğu için boş olması hata vermez ama temizlik iyidir)
                ModelState.Remove("SelectedDeliveryAddressId");
            }

            if (!ModelState.IsValid)
            {
                // View için gerekli verileri tekrar yükle
                vm.DeliveryAddresses = _addressService.GetUserAddresses(userId, AddressType.Delivery).ToList();
                vm.InvoiceAddresses = _addressService.GetUserAddresses(userId, AddressType.Invoice).ToList();

                var jsonStringView = System.IO.File.ReadAllText("wwwroot/data/il_ilce_mahalle.json");
                var jsonDataView = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<string>>>>(jsonStringView);
                vm.Provinces = jsonDataView?.Select(c => new Province
                {
                    Name = c.Key,
                    Districts = c.Value.Select(d => new District
                    {
                        Name = d.Key,
                        Neighborhoods = d.Value
                    }).OrderBy(d => d.Name).ToList()
                }).OrderBy(p => p.Name).ToList() ?? new List<Province>();

                return View(vm);
            }

            if (_cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sepetiniz boş.");
                // Tekrar verileri yükle (yukarıdaki blokla aynı, ama basitlik için kalsın veya fonksiyona çıkarılabilir)
                return View(vm);
            }

            // Teslimat adresini belirle
            Address? deliveryAddress = null;

            if (vm.UseNewAddress)
            {
                deliveryAddress = vm.Address;
                deliveryAddress.UserId = userId;
                deliveryAddress.AddressType = AddressType.Delivery;
            }
            else
            {
                // Kayıtlı adres seçildiyse onu getir (ModelState.IsValid geçtiği için SelectedDeliveryAddressId dolu olmalı)
                if (!vm.SelectedDeliveryAddressId.HasValue)
                {
                    ModelState.AddModelError("", "Lütfen bir teslimat adresi seçiniz.");
                    return View(vm);
                }
                deliveryAddress = _addressService.GetAddressById(vm.SelectedDeliveryAddressId.Value, userId);
                if (deliveryAddress == null)
                {
                    ModelState.AddModelError("", "Seçilen teslimat adresi bulunamadı.");
                    return View(vm);
                }
            }

            // Order Name'i otomatik oluştur
            vm.Order.Name = $"{deliveryAddress.FirstName} {deliveryAddress.LastName} - {DateTime.Now:dd/MM/yyyy HH:mm}";

            // Fiyat kontrolü
            var totalPrice = _cart.Lines.Sum(l => l.Product.Price * l.Quantity);
            if (totalPrice <= 0)
            {
                ModelState.AddModelError("", "Fiyat bilgisi sıfırdan büyük olmalıdır.");
                return View(vm);
            }

            // Adres bilgilerini Order'a kopyala (snapshot)
            vm.Order.UserId = userId;
            vm.Order.ShippingFirstName = deliveryAddress.FirstName;
            vm.Order.ShippingLastName = deliveryAddress.LastName;
            vm.Order.ShippingPhone = deliveryAddress.PhoneNumber;
            vm.Order.ShippingProvince = deliveryAddress.Province;
            vm.Order.ShippingDistrict = deliveryAddress.District;
            vm.Order.ShippingNeighborhood = deliveryAddress.Neighborhood;
            vm.Order.ShippingPostalCode = deliveryAddress.PostalCode;
            vm.Order.ShippingAddressDetail = deliveryAddress.AddressDetail;

            // CartLine'ları Order nesnesine ata
            vm.Order.Lines = _cart.Lines.Select(l => new CartLine
            {
                ProductId = l.Product.Id,
                Quantity = l.Quantity,
                Product = l.Product
            }).ToList();

            _orderService.SaveOrder(vm.Order);

            var payment = await _paymentService.MakePayment(
                vm.Order,
                deliveryAddress,
                vm.CardName,
                vm.CardNumber,
                vm.ExpireMonth,
                vm.ExpireYear,
                vm.Cvc
            );

            if (payment.Status == "success")
            {
                // Yeni adres girildiyse ve kaydet seçeneği işaretliyse kaydet
                if (vm.UseNewAddress && deliveryAddress.Id == 0 && SaveNewAddress)
                {
                    _addressService.CreateAddress(deliveryAddress);
                }

                // Siparişi tamamla
                await _orderService.CompleteAsync(vm.Order.Id);

                // Müşteriye sipariş onay maili gönder
                var user = await _userManager.GetUserAsync(User);
                if (user != null && !string.IsNullOrEmpty(user.Email))
                {
                    try
                    {
                        await _mailService.SendOrderConfirmationAsync(user.Email, vm.Order);
                    }
                    catch (Exception ex)
                    {
                        // Mail gönderimi başarısız olsa bile sipariş süreci devam etmeli
                        // Loglama yapılabilir
                        Console.WriteLine($"Mail gönderilemedi: {ex.Message}");
                    }
                }

                _cart.Clear();
                TempData["OrderSuccess"] = "Siparişiniz başarıyla alındı! En kısa sürede kargoya verilecektir.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Ödeme başarısız: " + payment.ErrorMessage);
            }

            return View(vm);
        }
    }
}
