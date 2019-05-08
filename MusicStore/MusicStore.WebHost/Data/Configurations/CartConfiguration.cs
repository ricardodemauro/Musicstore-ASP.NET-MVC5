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

            builder.Ignore(x => x.CartId);
            builder.Ignore(x => x.Album);
            builder.Ignore(x => x.AlbumId);
        }
    }
}
