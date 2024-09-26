using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.HasMany(o => o.OrderDetails).WithOne(od => od.Order).HasForeignKey(od => od.OrderId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.User).WithMany(o => o.Order).HasForeignKey(o => o.UserId);

            builder.Property(o => o.Id).ValueGeneratedOnAdd();

            builder.Property(o => o.Adress).HasColumnType("VARCHAR").HasMaxLength(50).IsRequired();
        }
    }
}
