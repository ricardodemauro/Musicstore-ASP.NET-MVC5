using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicStore.WebHost.Data;
using MusicStore.WebHost.Models;

namespace MusicStore.WebHost.Repositories
{
    public class EFGenreRepository : IGenreRepository
    {
        private readonly MusicStoreDbContext _dbContext;

        public EFGenreRepository(MusicStoreDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IReadOnlyList<Album>> AlbumsFromGenre(string genreName, CancellationToken cancellationToken = default)
        {
            Genre genreWithAlbums = await _dbContext
                .Genres.Include(x => x.Albums)
                .SingleAsync(p => p.Name == genreName, cancellationToken);

            return genreWithAlbums.Albums;
        }

        public async Task<IReadOnlyList<Genre>> ToList(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Genres.ToListAsync(cancellationToken);
        }
    }
}
