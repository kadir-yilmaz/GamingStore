using GamingStore.EL.Models.GamingStore.EL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingStore.EL.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public ICollection<CartLine> Lines { get; set; } = new List<CartLine>();

        public String? Name { get; set; }

        // Teslimat adresi (snapshot - sipariş anında kopyalanır)
        public string? ShippingFirstName { get; set; }
        public string? ShippingLastName { get; set; }
        public string? ShippingPhone { get; set; }
        public string? ShippingProvince { get; set; }
        public string? ShippingDistrict { get; set; }
        public string? ShippingNeighborhood { get; set; }
        public string? ShippingPostalCode { get; set; }
        public string? ShippingAddressDetail { get; set; }

        public bool GiftWrap { get; set; }
        public bool Shipped { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string? CargoCompany { get; set; }
        public string? TrackingNumber { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime OrderedAt { get; set; } = DateTime.Now;
    }
}


