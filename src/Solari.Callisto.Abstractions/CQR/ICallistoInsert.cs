using MongoDB.Driver;

namespace Solari.Callisto.Abstractions.CQR
{
    public interface ICallistoInsert<T> : ICallistoOperation<T> where T : class, IDocumentRoot
    {
        T Value { get; }
        InsertOneOptions InsertOneOptions { get; }
    }
}