using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using System.Reflection.Emit;

namespace WebApplication1.Config
{
    public class CateogryConfiguration : IEntityTypeConfiguration<Cateogry>
    {
        public void Configure(EntityTypeBuilder<Cateogry> builder)
        {
            builder.HasMany(c => c.Product).WithOne(p => p.Cateogry).HasForeignKey(p => p.CateogryId).OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.Name).HasColumnType("VARCHAR").HasMaxLength(50).IsRequired();

            // Seed data for Cateogry
            builder.HasData(
                new Cateogry { Id = 1, Name = "Electronics" },
                new Cateogry { Id = 2, Name = "Clothing" },
                new Cateogry { Id = 3, Name = "Groceries" }
            );

       
        }
    }
}
