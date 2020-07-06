using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Solari.Callisto.Abstractions.Contracts
{
    public interface ICallistoClient
    {
        string ConnectionString { get; }
        IMongoClient MongoClient { get; }
        Task<CallistoConnectionCheck> IsConnected(string database = "admin", CancellationToken? cancellationToken = null);
    }
}
