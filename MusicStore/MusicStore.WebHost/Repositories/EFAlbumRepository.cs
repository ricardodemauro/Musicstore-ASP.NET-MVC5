using Microsoft.EntityFrameworkCore;
using MusicStore.WebHost.Data;
using MusicStore.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStore.WebHost.Repositories
{
    public class EFAlbumRepository : IAlbumRepository
    {
        private readonly MusicStoreDbContext _dbContext;

        public EFAlbumRepository(MusicStoreDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Album> Create(Album album, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(album, cancellationToken).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return album;
        }

        public async Task<bool> Delete(Album album, CancellationToken cancellationToken = default)
        {
            _dbContext.Albums.Remove(album);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }

        public Task<Album> FindAlbum(int albumId, CancellationToken cancellationToken = default)
        {
            return _dbContext.Albums.FirstOrDefaultAsync(x => x.AlbumId == albumId, cancellationToken);
        }

        public async Task<IReadOnlyList<Album>> GetTopSellingAlbums(int count, CancellationToken cancellationToken = default)
        {
            // Group the order details by album and return
            // the albums with the highest count
            return await _dbContext.Albums
                .OrderByDescending(a => a.OrderDetails.Count())
                .Take(count)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<IReadOnlyCollection<Album>> ToListWithDetails(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Albums
                .Include(a => a.Artist)
                .Include(a => a.Genre)
                .ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<Album> Update(Album album, CancellationToken cancellationToken = default)
        {
            _dbContext.Entry(album).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return album;
        }
    }
}