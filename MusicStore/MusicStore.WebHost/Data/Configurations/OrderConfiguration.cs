using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStore.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.WebHost.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");

            builder.HasKey(x => x.OrderId)
                .HasName("PK_Order");

            builder.Property(x => x.Username).HasMaxLength(200);

            builder.Property(x => x.OrderDate);

            builder.Property(x => x.FirstName).HasMaxLength(160);

            builder.Property(x => x.LastName).HasMaxLength(160);

            builder.Property(x => x.Address).HasMaxLength(160);

            builder.Property(x => x.City).HasMaxLength(40);

            builder.Property(x => x.State).HasMaxLength(40);

            builder.Property(x => x.PostalCode).HasMaxLength(10);

            builder.Property(x => x.Country).HasMaxLength(40);

            builder.Property(x => x.Phone).HasMaxLength(24);

            builder.Property(x => x.Email).HasMaxLength(160);

            builder.Property(x => x.Total);

            builder.Property(x => x.PromoCode).HasMaxLength(160);
        }
    }
}
