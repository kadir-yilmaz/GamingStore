using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingStore.EL.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; } // navigation property
    }
}
