using MongoDB.Driver;

namespace Solari.Callisto.Abstractions.Contracts.CQR
{
    public interface ICallistoDelete<T> : ICallistoOperation<T> where T : class, IDocumentRoot
    {
        FilterDefinition<T> FilterDefinition { get; }
        DeleteOptions DeleteOptions { get; }
    }
}