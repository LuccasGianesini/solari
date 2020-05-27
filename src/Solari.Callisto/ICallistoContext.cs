using MongoDB.Driver;
using SharpCompress;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Connector;
using Solari.Callisto.Framework;

namespace Solari.Callisto
{
    public interface ICallistoContext<TCollection> where TCollection : class, IDocumentRoot
    {
        ICallistoInsertOperationFactory InsertOperationFactory { get; }
         ICallistoUpdateOperationFactory UpdateOperationFactory { get; }
         ICallistoDeleteOperationFactory DeleteOperationFactory { get; }
         ICallistoReplaceOperationFactory ReplaceOperationFactory { get; }
         ICallistoQueryOperationFactory QueryOperationFactory { get; }
        ICallistoConnection Connection { get; }
        IMongoCollection<TCollection> Collection { get; }
    }
}