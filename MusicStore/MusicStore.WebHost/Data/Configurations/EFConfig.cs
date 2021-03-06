﻿using Microsoft.EntityFrameworkCore;
using MusicStore.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.WebHost.Data.Configurations
{
    internal static class EFConfig
    {
        internal static readonly IEntityTypeConfiguration<Album> Album = new AlbumConfiguration();

        internal static readonly IEntityTypeConfiguration<Genre> Genre = new GenreConfiguration();

        internal static readonly IEntityTypeConfiguration<Artist> Artist = new ArtistConfiguration();

        internal static readonly IEntityTypeConfiguration<Cart> Cart = new CartConfiguration();

        internal static readonly IEntityTypeConfiguration<Order> Order = new OrderConfiguration();

        internal static readonly IEntityTypeConfiguration<OrderDetail> OrderDetail = new OrderDetailsConfiguration();
    }
}
