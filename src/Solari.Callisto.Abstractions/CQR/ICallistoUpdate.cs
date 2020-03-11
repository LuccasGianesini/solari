using MongoDB.Driver;

namespace Solari.Callisto.Abstractions.CQR
{
    public interface ICallistoUpdate<T> : ICallistoOperation<T> where T : class, IDocumentRoot
    {
        UpdateDefinition<T> UpdateDefinition { get; }
        FilterDefinition<T> FilterDefinition { get; }
        UpdateOptions UpdateOptions { get; }
    }
}