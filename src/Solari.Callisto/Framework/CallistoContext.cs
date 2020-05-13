using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Connector;

namespace Solari.Callisto.Framework
{
    public class CallistoContext<TCollection> : ICallistoContext<TCollection> where TCollection : class, IDocumentRoot
    {
        public CallistoContext(IMongoCollection<TCollection> collection, ICallistoConnection connection, ICallistoOperationFactory operationFactory)
        {
            Collection = collection;
            OperationFactory = operationFactory;
            Connection = connection;
        }
        public ICallistoConnection Connection { get; }
        public IMongoCollection<TCollection> Collection { get; }
        public ICallistoOperationFactory OperationFactory { get; }
    }
}