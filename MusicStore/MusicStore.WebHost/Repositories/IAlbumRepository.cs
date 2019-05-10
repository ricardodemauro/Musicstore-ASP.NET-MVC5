using MusicStore.WebHost.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStore.WebHost.Repositories
{
    public interface IAlbumRepository
    {
        Task<IReadOnlyList<Album>> GetTopSellingAlbums(int count, CancellationToken cancellationToken = default);

        Task<Album> FindAlbum(int albumId, CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<Album>> ToListWithDetails(CancellationToken cancellationToken = default);

        Task<bool> Delete(Album album, CancellationToken cancellationToken = default);

        Task<Album> Create(Album album, CancellationToken cancellationToken = default);

        Task<Album> Update(Album album, CancellationToken cancellationToken = default);
    }
}
