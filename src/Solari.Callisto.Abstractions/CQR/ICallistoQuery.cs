using System;
using MongoDB.Driver;

namespace Solari.Callisto.Abstractions.CQR
{
    public interface ICallistoQuery<T, out TResult> : ICallistoOperation<T> where T : class, IDocumentRoot
    {
        FindOptions<T> FindOptions { get; }
        FilterDefinition<T> FilterDefinition { get; }
        Func<IAsyncCursor<T>, TResult> ResultFunction { get; }
        
    }
}