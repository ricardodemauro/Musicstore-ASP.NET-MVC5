using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStore.WebHost.Infrastructure.Providers
{
    public class HttpSessionProvider : ISessionProvider
    {
        private readonly ISession _session;
        private bool _loaded = false;

        public HttpSessionProvider(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public async Task<T> GetDataAsync<T>(string key, CancellationToken cancellationToken = default(CancellationToken)) where T : class, new()
        {
            if (!_loaded)
                await _session.LoadAsync(cancellationToken);
            return Get<T>(_session, key);
        }

        public Task SetDataAsync<T>(string key, T data, CancellationToken cancellationToken = default(CancellationToken))
        {
            Set<T>(_session, key, data);

            return _session.CommitAsync(cancellationToken);
        }

        private static void Set<T>(ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        private static T Get<T>(ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<string> GetStringAsync(string key, CancellationToken cancellationToken = default)
        {
            if (!_loaded)
                await _session.LoadAsync(cancellationToken);
            return _session.GetString(key);
        }

        public Task SetStringAsync(string key, string data, CancellationToken cancellationToken = default)
        {
            _session.SetString(key, data);

            return _session.CommitAsync(cancellationToken);
        }

        public async Task<int?> GetIntegerAsync(string key, CancellationToken cancellationToken = default)
        {
            if (!_loaded)
                await _session.LoadAsync(cancellationToken);
            return _session.GetInt32(key);
        }

        public async Task SetIntegerAsync(string key, int data, CancellationToken cancellationToken = default)
        {
            _session.SetInt32(key, data);

            await _session.CommitAsync(cancellationToken);
        }
    }
}
