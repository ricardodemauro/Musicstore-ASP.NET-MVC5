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
            builder.ToTable("Album");

            builder.HasKey(x => x.AlbumId)
                .HasName("PK_Album");

            builder.Property(x => x.Title)
                .HasMaxLength(160);

            builder.Property(x => x.Price)
                .HasColumnType("money");

            builder.Property(x => x.AlbumArtUrl)
                .HasMaxLength(1024);


            builder.HasOne(x => x.Genre)
                .WithMany(x => x.Albums)
                .HasForeignKey(x => x.GenreId)
                .HasConstraintName("FK_Genre_Album");

            builder.HasOne(x => x.Artist)
                .WithMany(x => x.Albums)
                .HasForeignKey(x => x.ArtistId)
                .HasConstraintName("FK_Album_Artist");
        }
    }
}
