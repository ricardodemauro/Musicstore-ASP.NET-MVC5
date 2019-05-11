using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicStore.WebHost.Data.Configurations;
using MusicStore.WebHost.Models;

namespace MusicStore.WebHost.Data
{
    public class MusicStoreDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Album> Albums { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Artist> Artists { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public MusicStoreDbContext(DbContextOptions<MusicStoreDbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(EFConfig.Genre);
            builder.ApplyConfiguration(EFConfig.Artist);
            builder.ApplyConfiguration(EFConfig.Album);
            builder.ApplyConfiguration(EFConfig.Cart);

            builder.ApplyConfiguration(EFConfig.Order);
            builder.ApplyConfiguration(EFConfig.OrderDetail);

            base.OnModelCreating(builder);
        }
    }
}