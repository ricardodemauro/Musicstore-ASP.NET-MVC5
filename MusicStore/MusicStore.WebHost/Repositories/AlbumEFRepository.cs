using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicStore.EntityContext;
using MusicStore.Models;

namespace MusicStore.Repositories
{
    public class AlbumEFRepository : IAlbumRepository
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();

        public List<Album> GetTopSellingAlbums(int count)
        {
            // Group the order details by album and return
            // the albums with the highest count
            return storeDB.Albums
                .OrderByDescending(a => a.OrderDetails.Count())
                .Take(count)
                .ToList();
        }
    }
}