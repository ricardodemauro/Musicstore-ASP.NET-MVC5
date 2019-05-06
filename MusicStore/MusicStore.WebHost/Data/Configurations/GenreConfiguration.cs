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
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasKey(x => x.GenreId);

            builder.Property(x => x.Name)
                .HasMaxLength(260);

            builder.Property(x => x.Description)
                .HasMaxLength(1024);

            builder.HasMany(x => x.Albums)
                .WithOne(x => x.Genre)
                .OnDelete(DeleteBehavior.SetNull)
                .HasForeignKey(x => x.GenreId);
        }
    }
}
