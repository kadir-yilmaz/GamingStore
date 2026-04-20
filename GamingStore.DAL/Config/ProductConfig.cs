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
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Price).IsRequired();

            builder.HasData(
                // Laptop
                new Product() { Id = 1, CategoryId = 1, ImageURL = "/images/laptop1.jpg", Name = "Laptop A", Price = 15000, Showcase = true, Stock = 11 },
                new Product() { Id = 2, CategoryId = 1, ImageURL = "/images/laptop2.jpg", Name = "Laptop B", Price = 17000, Showcase = false, Stock = 0 }, // Stok 0
                new Product() { Id = 3, CategoryId = 1, ImageURL = "/images/laptop3.jpg", Name = "Laptop C", Price = 20000, Showcase = false, Stock = 30 },

                // Desktop
                new Product() { Id = 4, CategoryId = 2, ImageURL = "/images/desktop1.jpg", Name = "Desktop A", Price = 18000, Showcase = true, Stock = 20 },
                new Product() { Id = 5, CategoryId = 2, ImageURL = "/images/desktop2.jpg", Name = "Desktop B", Price = 16000, Showcase = false, Stock = 15 },
                new Product() { Id = 6, CategoryId = 2, ImageURL = "/images/desktop3.jpg", Name = "Desktop C", Price = 19000, Showcase = false, Stock = 25 },

                // Keyboard
                new Product() { Id = 7, CategoryId = 3, ImageURL = "/images/keyboard1.jpg", Name = "Keyboard A", Price = 500, Showcase = true, Stock = 100 },
                new Product() { Id = 8, CategoryId = 3, ImageURL = "/images/keyboard2.jpg", Name = "Keyboard B", Price = 750, Showcase = false, Stock = 80 },
                new Product() { Id = 9, CategoryId = 3, ImageURL = "/images/keyboard3.jpg", Name = "Keyboard C", Price = 600, Showcase = false, Stock = 90 },

                // Mouse
                new Product() { Id = 10, CategoryId = 4, ImageURL = "/images/mouse1.jpg", Name = "Mouse A", Price = 300, Showcase = true, Stock = 150 },
                new Product() { Id = 11, CategoryId = 4, ImageURL = "/images/mouse2.jpg", Name = "Mouse B", Price = 450, Showcase = false, Stock = 120 },
                new Product() { Id = 12, CategoryId = 4, ImageURL = "/images/mouse3.jpg", Name = "Mouse C", Price = 400, Showcase = false, Stock = 130 },

                // Mouse Pad
                new Product() { Id = 13, CategoryId = 5, ImageURL = "/images/mp1.jpg", Name = "Mouse Pad A", Price = 150, Showcase = true, Stock = 200 },
                new Product() { Id = 14, CategoryId = 5, ImageURL = "/images/mp2.jpg", Name = "Mouse Pad B", Price = 200, Showcase = false, Stock = 180 },
                new Product() { Id = 15, CategoryId = 5, ImageURL = "/images/mp3.jpg", Name = "Mouse Pad C", Price = 180, Showcase = false, Stock = 190 },

                // Monitor
                new Product() { Id = 16, CategoryId = 6, ImageURL = "/images/monitor1.jpg", Name = "Monitor A", Price = 5000, Showcase = true, Stock = 40 },
                new Product() { Id = 17, CategoryId = 6, ImageURL = "/images/monitor2.jpg", Name = "Monitor B", Price = 7000, Showcase = false, Stock = 35 },
                new Product() { Id = 18, CategoryId = 6, ImageURL = "/images/monitor3.jpg", Name = "Monitor C", Price = 6500, Showcase = false, Stock = 45 }
            );


        }
    }
}
