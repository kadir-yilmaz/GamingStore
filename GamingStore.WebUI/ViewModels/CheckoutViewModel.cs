using AddressModel = GamingStore.EL.Models.Address;
using GamingStore.EL.Models;
using GamingStore.WebUI.ViewModels.Address;
using GamingStore.WebUI.ViewModels.Location;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GamingStore.WebUI.ViewModels
{
    public class CheckoutViewModel : IAddressViewModel
    {
        public Order Order { get; set; } = new();
        
        // Mevcut adres yapısı (yeni adres girme için veya kayıtlı adres yoksa)
        public AddressModel Address { get; set; } = new();

        // Kayıtlı adres seçimi
        public int? SelectedDeliveryAddressId { get; set; }
        public int? SelectedInvoiceAddressId { get; set; }
        public bool UseSameAddressForInvoice { get; set; } = true;
        
        // Kayıtlı adres listeleri
        public List<AddressModel> DeliveryAddresses { get; set; } = new();
        public List<AddressModel> InvoiceAddresses { get; set; } = new();
        
        // Yeni adres mi girilecek?
        public bool UseNewAddress { get; set; } = false;

        // Ödeme bilgileri
        [Required(ErrorMessage = "Kart üzerindeki isim gereklidir.")]
        public string CardName { get; set; }

        [Required(ErrorMessage = "Kart numarası gereklidir.")]
        [CreditCard(ErrorMessage = "Geçersiz kart numarası.")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Son kullanma ayı gereklidir.")]
        [RegularExpression(@"^(0[1-9]|1[0-2])$", ErrorMessage = "Geçersiz ay (01-12).")]
        public string ExpireMonth { get; set; }

        [Required(ErrorMessage = "Son kullanma yılı gereklidir.")]
        [RegularExpression(@"^20[2-9][0-9]$", ErrorMessage = "Geçersiz yıl.")]
        public string ExpireYear { get; set; }

        [Required(ErrorMessage = "CVC kodu gereklidir.")]
        [RegularExpression(@"^[0-9]{3,4}$", ErrorMessage = "Geçersiz CVC.")]
        public string Cvc { get; set; }

        // İl-ilçe-mahalle için JSON verisi
        public List<Province> Provinces { get; set; } = new();
    }
}
