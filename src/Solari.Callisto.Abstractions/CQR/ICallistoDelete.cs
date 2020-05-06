using MongoDB.Driver;

namespace Solari.Callisto.Abstractions.CQR
{
    public interface ICallistoDelete<T> : ICallistoOperation<T> where T : class, IDocumentRoot
    {
        FilterDefinition<T> FilterDefinition { get; }
        DeleteOptions DeleteOptions { get; }
    }
}