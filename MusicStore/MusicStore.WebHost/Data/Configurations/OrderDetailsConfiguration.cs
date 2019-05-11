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
    public class OrderDetailsConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetail");

            builder.HasKey(x => x.OrderDetailId)
                .HasName("PK_OrderDetail");

            builder.Property(x => x.OrderId);

            builder.Property(x => x.AlbumId);

            builder.Property(x => x.Quantity);

            builder.Property(x => x.UnitPrice);
        }
    }
}
