using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingStore.EL.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock {  get; set; }
        public string? Summary { get; set; } = string.Empty;
        public string? ImageURL { get; set; }
        public bool Showcase { get; set; }
        public int? CategoryId { get; set; } // foreign key
        public Category? Category { get; set; } // navigation property
    }
}
