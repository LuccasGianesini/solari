using MongoDB.Driver;
using SharpCompress;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Connector;

namespace Solari.Callisto.Framework
{
    public class CallistoContext<TCollection> : ICallistoContext<TCollection> where TCollection : class, IDocumentRoot
    {
        private readonly System.Lazy<ICallistoInsertOperationFactory> _insert =
            new System.Lazy<ICallistoInsertOperationFactory>(() => new CallistoInsertOperationFactory());

        private readonly System.Lazy<ICallistoUpdateOperationFactory> _update =
            new System.Lazy<ICallistoUpdateOperationFactory>(() => new CallistoUpdateOperationFactory());

        private readonly System.Lazy<ICallistoDeleteOperationFactory> _delete =
            new System.Lazy<ICallistoDeleteOperationFactory>(() => new CallistoDeleteOperationFactory());

        private readonly System.Lazy<ICallistoReplaceOperationFactory> _replace =
            new System.Lazy<ICallistoReplaceOperationFactory>(() => new CallistoReplaceOperationFactory());

        private readonly System.Lazy<ICallistoQueryOperationFactory> _query =
            new System.Lazy<ICallistoQueryOperationFactory>(() => new CallistoQueryOperationFactory());

        public CallistoContext(IMongoCollection<TCollection> collection, ICallistoConnection connection)
        {
            Collection = collection;
            Connection = connection;
        }

        public ICallistoConnection Connection { get; }
        public IMongoCollection<TCollection> Collection { get; }

        public ICallistoInsertOperationFactory InsertOperationFactory => _insert.Value;

        public ICallistoUpdateOperationFactory UpdateOperationFactory => _update.Value;
        public ICallistoDeleteOperationFactory DeleteOperationFactory => _delete.Value;
        public ICallistoReplaceOperationFactory ReplaceOperationFactory => _replace.Value;
        public ICallistoQueryOperationFactory QueryOperationFactory => _query.Value;
    }
}