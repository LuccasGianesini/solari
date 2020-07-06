using MongoDB.Driver;

namespace Solari.Callisto.Abstractions.Contracts
{
    public interface ICallistoCollectionContext<T> where T : class, IDocumentRoot
    {
        ICallistoClient CallistoClient { get; }
        IMongoCollection<T> Collection { get; }
        IMongoDatabase Database { get; }

    }
}
