﻿using MusicStore.WebHost.Data;
using MusicStore.WebHost.Models;
using System.Collections.Generic;
using System.Linq;

namespace MusicStore.WebHost.Repositories
{
    public class EFAlbumRepository : IAlbumRepository
    {
        private readonly MusicStoreDbContext _dbContext;

        public EFAlbumRepository(MusicStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IReadOnlyList<Album> GetTopSellingAlbums(int count)
        {
            // Group the order details by album and return
            // the albums with the highest count
            return _dbContext.Albums
                .OrderByDescending(a => a.OrderDetails.Count())
                .Take(count)
                .ToList();
        }
    }
}