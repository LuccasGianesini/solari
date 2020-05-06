using System.Collections.Generic;
using MongoDB.Driver;

namespace Solari.Callisto.Abstractions.CQR
{
    public interface ICallistoInsertMany<T> : ICallistoOperation<T> where T : class, IDocumentRoot
    {
        IEnumerable<T> Values { get; }
        InsertManyOptions InsertManyOptions { get; }
    }
}