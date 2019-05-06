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
    public class AlbumConfiguration : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.ToTable("album");

            builder.HasKey(x => x.AlbumId)
                .HasName("PK_album");

            builder.Property(x => x.Title)
                .HasMaxLength(160);

            builder.Property(x => x.Price)
                .HasColumnType("money");

            builder.Property(x => x.AlbumArtUrl)
                .HasMaxLength(1024);
        }
    }
}
