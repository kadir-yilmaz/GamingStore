using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingStore.EL.Models
{
    public class Address
    {
        public int Id { get; set; }

        // Kullanıcı ilişkisi (nullable - mevcut siparişler için)
        public string? UserId { get; set; }

        // Adres tipi (Teslimat veya Fatura)
        public AddressType AddressType { get; set; } = AddressType.Delivery;

        // Adres kategorisi (Ev veya İş yeri)
        public AddressCategory Category { get; set; } = AddressCategory.Home;

        // İş yeri için kapalı günler (virgülle ayrılmış: "Cumartesi,Pazar")
        public string? ClosedDays { get; set; }

        // Adres başlığı (Evim, İşim, vb.)
        [Required(ErrorMessage = "Adres başlığı gereklidir.")]
        public string Title { get; set; } = "Evim";

        // Varsayılan adres mi?
        public bool IsDefault { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Province is required.")]
        public string? Province { get; set; }

        [Required(ErrorMessage = "District is required.")]
        public string? District { get; set; }

        [Required(ErrorMessage = "Neighborhood is required.")]
        public string? Neighborhood { get; set; }

        [Required(ErrorMessage = "Postal code is required.")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Postal code must be 5 digits.")]
        public string? PostalCode { get; set; }

        [Required(ErrorMessage = "Address details are required.")]
        public string? AddressDetail { get; set; }
    }
}
