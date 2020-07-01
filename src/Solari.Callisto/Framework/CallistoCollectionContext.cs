using MongoDB.Driver;
using SharpCompress;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Connector;

namespace Solari.Callisto.Framework
{
    public class CallistoCollectionContext<T> : ICallistoCollectionContext<T> where T : class, IDocumentRoot
    {
        public CallistoCollectionContext(IMongoCollection<T> collection,
                                         ICallistoConnection connection,
                                         ICollectionOperators<T> operators)
        {
            Collection = collection;
            Connection = connection;
            Operators = operators;
        }

        public ICallistoConnection Connection { get; }
        public IMongoCollection<T> Collection { get; }
        public ICollectionOperators<T> Operators { get; }
    }
}
