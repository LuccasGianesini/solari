using System;
using MongoDB.Driver;

namespace Solari.Callisto.Abstractions.Contracts.CQR
{

    public interface ICallistoQuery<T> : ICallistoOperation<T> where T : class, IDocumentRoot
    {
        FindOptions<T> FindOptions { get; }
        FilterDefinition<T> FilterDefinition { get; }
    }

    public interface ICallistoExecutableQuery<T, out TResult> : ICallistoOperation<T> where T : class, IDocumentRoot
    {
        FindOptions<T> FindOptions { get; }
        FilterDefinition<T> FilterDefinition { get; }
        Func<IAsyncCursor<T>, TResult> ResultFunction { get; }
    }
}
