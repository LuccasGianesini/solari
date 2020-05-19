using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Connector;

namespace Solari.Callisto
{
    public interface ICallistoContext<TCollection> where TCollection : class, IDocumentRoot
    {
        ICallistoOperationFactory OperationFactory { get; }
        ICallistoConnection Connection { get; }
        IMongoCollection<TCollection> Collection { get; }
    }
}