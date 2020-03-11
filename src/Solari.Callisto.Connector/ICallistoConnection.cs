using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Solari.Callisto.Connector
{
    public interface ICallistoConnection
    {

        IMongoDatabase GetDataBase();
        IMongoClient LockedRead();
        IMongoClient GetClient();
        string DataBaseName { get; }
        void UpdateClient(IMongoClient client);
        void ChangeDatabase(string dataBaseName);
        Task<CallistoConnectionCheck> IsConnected(CancellationToken? cancellationToken = null);
        ICallistoConnection AddClient(IMongoClient mongoClient);
        ICallistoConnection AddDataBase(string dataBaseName);
    }
}