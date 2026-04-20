using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingStore.EL.Dtos
{
    public record ProductDto
    {
        public int Id { get; init; }

        [Required(ErrorMessage = "ProductName is required.")]
        public String? Name { get; init; } = String.Empty;

        [Required(ErrorMessage = "Price is required.")]
        public decimal Price { get; init; }
        public int Stock { get; init; }
        public String? Summary { get; init; } = String.Empty;
        public String? ImageURL { get; set; } // atama işlemi sonra yapılacak set olarak bıraktık
        public int? CategoryId { get; init; } = 1;
    }
}
