using MusicStore.WebHost.Models;
using System.Collections.Generic;

namespace MusicStore.WebHost.Repositories
{
    public interface IAlbumRepository
    {
        List<Album> GetTopSellingAlbums(int count);
    }
}
