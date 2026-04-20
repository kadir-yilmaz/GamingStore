using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingStore.EL.Models
{
    namespace GamingStore.EL.Models
    {
        public class CartLine
        {
            public int Id { get; set; }
            public int ProductId { get; set; }
            public Product Product { get; set; } = new();
            public int Quantity { get; set; }
        }
    }
}
