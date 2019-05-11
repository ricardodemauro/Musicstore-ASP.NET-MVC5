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
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Cart");

            builder.HasKey(x => x.RecordId)
                .HasName("PK_Record");

            builder.Property(x => x.Count);

            builder.Property(x => x.DateCreated);

            builder.Property(x => x.CartId);

            builder.HasOne(x => x.Album);
            //builder.Property(x => x.AlbumId);
        }
    }
}
