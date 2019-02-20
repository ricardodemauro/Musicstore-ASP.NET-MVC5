using MusicStore.Models;
using MusicStore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Tests.Mocks
{
    public class MockAlbumRepository : IAlbumRepository
    {
        public List<Album> GetTopSellingAlbums(int count)
        {
            return new List<Album>();
        }
    }
}
