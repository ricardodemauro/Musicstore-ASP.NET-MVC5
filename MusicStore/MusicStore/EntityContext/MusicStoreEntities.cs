using MusicStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MusicStore.EntityContext
{
    public class MusicStoreEntities : DbContext
    {
        /// <summary>
        /// Colecao de Albums
        /// </summary>
        public DbSet<Album> Albums { get; set; }

        /// <summary>
        /// Colecao de Generos
        /// </summary>
        public DbSet<Genre> Genres { get; set; }

        /// <summary>
        /// Colecao de Artistas
        /// </summary>
        public DbSet<Artist> Artists { get; set; }

        /// <summary>
        /// Colecao Carrinhos
        /// </summary>
        public DbSet<Cart> Carts { get; set; }


        /// <summary>
        /// Colecao Ordens
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// Colecao detalhe de ordens
        /// </summary>
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public MusicStoreEntities()
            : base("DefaultConnection")
        {

        }
    }
}