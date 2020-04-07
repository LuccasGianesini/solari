using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Solari.Miranda.Abstractions.Options;
using Solari.Oberon;

namespace Solari.Miranda.Framework
{
    /// <summary>
    /// FROM CONVEY
    /// </summary>
    internal sealed class RedisMessageProcessor : IMessageProcessor
    {
        private readonly IOberon _cache;
        private readonly MirandaOptions _options;
        private readonly string _service;

        public RedisMessageProcessor(IOberon cache, MirandaOptions options)
        {
            _cache = cache;
            _options = options;
            _service = string.IsNullOrWhiteSpace(options.Namespace)
                ? Guid.NewGuid().ToString("N")
                : options.Namespace;
        }

        public async Task<bool> TryProcessAsync(string id)
        {
            string key = GetKey(id);
            string message = await _cache.Cache.GetStringAsync(key);
            if (!string.IsNullOrWhiteSpace(message))
            {
                return false;
            }

            int expiry = _options.MessageProcessor?.MessageExpirySeconds ?? 0;
            if (expiry <= 0)
            {
                await _cache.Cache.SetStringAsync(key, id);
            }
            else
            {
                await _cache.Cache.SetStringAsync(key, id, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expiry)
                });
            }

            return true;
        }

        public Task RemoveAsync(string id) => _cache.Cache.RemoveAsync(GetKey(id));

        private string GetKey(string id) => $"messages:{_service}:{id}";
    }
}