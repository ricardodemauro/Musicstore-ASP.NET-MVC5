using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStore.WebHost.Infrastructure
{
    public interface ISessionProvider
    {
        Task<T> GetDataAsync<T>(string key, CancellationToken cancellationToken = default) where T : class, new();

        Task SetDataAsync<T>(string key, T data, CancellationToken cancellationToken = default);

        Task<string> GetStringAsync(string key, CancellationToken cancellationToken = default) where T : class, new();

        Task SetStringAsync(string key, string data, CancellationToken cancellationToken = default);

        Task<int?> GetIntegerAsync(string key, CancellationToken cancellationToken = default) where T : class, new();

        Task SetIntegerAsync(string key, int data, CancellationToken cancellationToken = default);
    }
}
