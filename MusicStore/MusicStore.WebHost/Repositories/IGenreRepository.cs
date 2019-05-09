using MusicStore.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStore.WebHost.Repositories
{
    public interface IGenreRepository
    {
        Task<IReadOnlyList<Genre>> ToList(CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Album>> AlbumsFromGenre(string genreName, CancellationToken cancellationToken = default);
    }
}
