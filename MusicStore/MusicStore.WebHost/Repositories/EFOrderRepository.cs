using Microsoft.EntityFrameworkCore;
using MusicStore.WebHost.Data;
using MusicStore.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStore.WebHost.Repositories
{
    public class EFOrderRepository : IOrderRepository
    {
        private readonly MusicStoreDbContext _dbContext;

        public EFOrderRepository(MusicStoreDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Order> AddOrder(Order item, CancellationToken cancellationToken = default)
        {
            _dbContext.Orders.Add(item);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return item;
        }

        public async Task<Order> FindOrder(int orderId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Orders.FirstOrDefaultAsync(x => x.OrderId == orderId, cancellationToken);
        }
    }
}
