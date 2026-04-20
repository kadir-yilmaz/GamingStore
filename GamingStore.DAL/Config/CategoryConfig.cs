using GamingStore.EL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingStore.DAL.Config
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name).IsRequired();

            var categories = new string[]
            {
    "Laptop", "Desktop", "Keyboard", "Mouse", "Mouse Pad",
    "Monitor", "Headset", "Processor", "Motherboard", "RAM",
    "Case", "Graphics Card", "SSD", "Power Supply", "Liquid Cooling"
            };

            builder.HasData(
                categories.Select((name, index) => new Category
                {
                    Id = index + 1, // Id 1'den başlasın
                    Name = name
                }).ToArray()
            );

        }
    }
}
