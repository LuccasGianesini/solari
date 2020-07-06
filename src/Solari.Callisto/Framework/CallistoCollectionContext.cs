using MongoDB.Driver;
using SharpCompress;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts;
using Solari.Callisto.Connector;

namespace Solari.Callisto.Framework
{
    public class CallistoCollectionContext<T> : ICallistoCollectionContext<T> where T : class, IDocumentRoot
    {
        public CallistoCollectionContext(ICallistoClient callistoClient, IMongoCollection<T> collection, IMongoDatabase database)
        {
            CallistoClient = callistoClient;
            Collection = collection;
            Database = database;
        }

        public ICallistoClient CallistoClient { get; }
        public IMongoCollection<T> Collection { get; }
        public IMongoDatabase Database { get; }
    }
}
