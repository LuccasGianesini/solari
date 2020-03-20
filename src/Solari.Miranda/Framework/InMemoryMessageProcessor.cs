using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Solari.Miranda.Abstractions.Options;

namespace Solari.Miranda.Framework
{
    /// <summary>
    /// FROM CONVEY
    /// </summary>
    public class InMemoryMessageProcessor : IMessageProcessor
    {
        private readonly IMemoryCache _cache;
        private readonly MirandaOptions _options;

        public InMemoryMessageProcessor(IMemoryCache cache, MirandaOptions options)
        {
            _cache = cache;
            _options = options;
        }

        public Task<bool> TryProcessAsync(string id)
        {
            var key = GetKey(_options.Namespace, id);
            if (_cache.TryGetValue(key, out _))
            {
                return Task.FromResult(false);
            }

            int expiry = _options.MessageProcessor?.MessageExpirySeconds ?? 0;
            _cache.Set(key, id, TimeSpan.FromSeconds(expiry));

            return Task.FromResult(true);
        }

        public Task RemoveAsync(string id)
        {
            _cache.Remove(GetKey(_options.Namespace, id));

            return Task.CompletedTask;
        }

        private static string GetKey(string @namespace, string id) => $"messages:{@namespace}:{id}";
    }
}