using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using System.Reflection.Emit;

namespace WebApplication1.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Name).HasColumnType("VARCHAR").HasMaxLength(50).IsRequired();
            
            builder.Property(p => p.Description).HasColumnType("VARCHAR").HasMaxLength(200).IsRequired(false);


            // Seed data for Product
            builder.HasData(
                new Product { Id = 1, Name = "Laptop", Price = 1200, Description = "High-end gaming laptop", CateogryId = 1 },
                new Product { Id = 2, Name = "Smartphone", Price = 800, Description = "Latest model smartphone", CateogryId = 1 },
                new Product { Id = 3, Name = "T-shirt", Price = 20, Description = "Cotton t-shirt", CateogryId = 2 },
                new Product { Id = 4, Name = "Jeans", Price = 50, Description = "Blue denim jeans", CateogryId = 2 },
                new Product { Id = 5, Name = "Apples", Price = 3, Description = "Fresh red apples", CateogryId = 3 }
            );
        }
    }
}
