using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Solari.Sol.Abstractions;

namespace Solari.Oberon
{
    public class Oberon : IOberon
    {
        public Oberon(IDistributedCache cache) { Cache = cache; }
        public IDistributedCache Cache { get; }


        public async Task<T> Read<T>(string key) where T : class
        {
            string str = await Cache.GetStringAsync(key);
            return SerializationHelper.Deserialize<T>(str);
        }

        public async Task Save<T>(string key, T obj, DistributedCacheEntryOptions options) where T : class
        {
            if (SerializationHelper.TrySerializeObject(obj, out string json)) await Cache.SetStringAsync(key, json, options);

            throw new OberonException("Unable to save object to cache");
        }
    }
}
