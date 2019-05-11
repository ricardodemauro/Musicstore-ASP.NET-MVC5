using MusicStore.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStore.WebHost.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> AddOrder(Order item, CancellationToken cancellationToken = default);

        Task<Order> FindOrder(int orderId, CancellationToken cancellationToken = default);
    }
}
