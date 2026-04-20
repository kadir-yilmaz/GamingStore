using GamingStore.BLL.Abstract;
using GamingStore.EL.Models;
using GamingStore.WebUI.ViewModels.Address;
using GamingStore.WebUI.ViewModels.Location;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GamingStore.WebUI.Controllers
{
    [Authorize]
    public class AddressController : Controller
    {
        private readonly IAddressService _addressService;
        private readonly UserManager<IdentityUser> _userManager;

        public AddressController(IAddressService addressService, UserManager<IdentityUser> userManager)
        {
            _addressService = addressService;
            _userManager = userManager;
        }

        private string GetUserId() => _userManager.GetUserId(User)!;

        /// <summary>
        /// Adres listesi sayfası (Teslimat / Fatura tab'ları)
        /// </summary>
        public IActionResult Index(AddressType? tab = null)
        {
            var userId = GetUserId();
            
            var model = new AddressListViewModel
            {
                DeliveryAddresses = _addressService.GetUserAddresses(userId, AddressType.Delivery).ToList(),
                InvoiceAddresses = _addressService.GetUserAddresses(userId, AddressType.Invoice).ToList(),
                ActiveTab = tab ?? AddressType.Delivery
            };

            return View(model);
        }

        /// <summary>
        /// Yeni adres ekleme sayfası
        /// </summary>
        public IActionResult Create(AddressType type = AddressType.Delivery)
        {
            var model = new AddressFormViewModel
            {
                Address = new Address { AddressType = type },
                Provinces = LoadProvinces()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AddressFormViewModel model)
        {
            model.Provinces = LoadProvinces();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = GetUserId();
            model.Address.UserId = userId;

            // İş yeri değilse kapalı günleri temizle
            if (model.Address.Category != AddressCategory.Workplace)
            {
                model.Address.ClosedDays = null;
            }

            _addressService.CreateAddress(model.Address);

            TempData["SuccessMessage"] = "Adres başarıyla eklendi.";
            return RedirectToAction(nameof(Index), new { tab = model.Address.AddressType });
        }

        /// <summary>
        /// Adres düzenleme sayfası
        /// </summary>
        public IActionResult Edit(int id)
        {
            var userId = GetUserId();
            var address = _addressService.GetAddressById(id, userId);

            if (address == null)
            {
                TempData["ErrorMessage"] = "Adres bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var model = new AddressFormViewModel
            {
                Address = address,
                Provinces = LoadProvinces()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AddressFormViewModel model)
        {
            model.Provinces = LoadProvinces();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = GetUserId();

            // İş yeri değilse kapalı günleri temizle
            if (model.Address.Category != AddressCategory.Workplace)
            {
                model.Address.ClosedDays = null;
            }

            try
            {
                _addressService.UpdateAddress(model.Address, userId);
                TempData["SuccessMessage"] = "Adres başarıyla güncellendi.";
            }
            catch (UnauthorizedAccessException)
            {
                TempData["ErrorMessage"] = "Bu adresi düzenleme yetkiniz yok.";
            }

            return RedirectToAction(nameof(Index), new { tab = model.Address.AddressType });
        }

        /// <summary>
        /// Adres silme
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, AddressType tab)
        {
            var userId = GetUserId();

            try
            {
                _addressService.DeleteAddress(id, userId);
                TempData["SuccessMessage"] = "Adres başarıyla silindi.";
            }
            catch (UnauthorizedAccessException)
            {
                TempData["ErrorMessage"] = "Bu adresi silme yetkiniz yok.";
            }

            return RedirectToAction(nameof(Index), new { tab });
        }

        /// <summary>
        /// Adresi varsayılan yap
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetDefault(int id, AddressType type)
        {
            var userId = GetUserId();

            try
            {
                _addressService.SetAsDefault(id, userId, type);
                TempData["SuccessMessage"] = "Varsayılan adres güncellendi.";
            }
            catch (UnauthorizedAccessException)
            {
                TempData["ErrorMessage"] = "Bu adresi varsayılan yapma yetkiniz yok.";
            }

            return RedirectToAction(nameof(Index), new { tab = type });
        }

        /// <summary>
        /// Checkout için adres listesi (AJAX)
        /// </summary>
        [HttpGet]
        public IActionResult GetForCheckout()
        {
            var userId = GetUserId();
            
            var deliveryAddresses = _addressService.GetUserAddresses(userId, AddressType.Delivery);
            var invoiceAddresses = _addressService.GetUserAddresses(userId, AddressType.Invoice);

            return Json(new
            {
                deliveryAddresses,
                invoiceAddresses
            });
        }

        private List<Province> LoadProvinces()
        {
            var jsonString = System.IO.File.ReadAllText("wwwroot/data/il_ilce_mahalle.json");
            var jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<string>>>>(jsonString);
            
            if (jsonData == null) return new List<Province>();

            return jsonData.Select(c => new Province
            {
                Name = c.Key,
                Districts = c.Value.Select(d => new District
                {
                    Name = d.Key,
                    Neighborhoods = d.Value
                }).OrderBy(d => d.Name).ToList()
            }).OrderBy(p => p.Name).ToList();
        }
    }
}
