using MongoDB.Driver;

namespace Solari.Callisto.Abstractions.CQR
{
    public interface ICallistoReplace<T> : ICallistoOperation<T> where T : class, IDocumentRoot
    {
        T Replacement { get; }
        FilterDefinition<T> FilterDefinition { get; }
        ReplaceOptions ReplaceOptions { get; }
    }
}