using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Solari.Oberon
{
    public interface IOberon
    {
        IDistributedCache Cache { get; }
        Task<T> Read<T>(string key) where T : class;
        Task Save<T>(string key, T obj, DistributedCacheEntryOptions options) where T : class;
    }
}