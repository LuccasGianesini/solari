using MongoDB.Driver;
using SharpCompress;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Connector;
using Solari.Callisto.Framework;

namespace Solari.Callisto
{
    public interface ICallistoCollectionContext<T> where T : class, IDocumentRoot
    {
        ICallistoConnection Connection { get; }
        IMongoCollection<T> Collection { get; }
        ICollectionOperators<T> Operators { get; }
    }
}
