using System.Collections.Generic;
using MongoDB.Driver;

namespace Solari.Callisto.Abstractions.Contracts.CQR
{
    public interface ICallistoInsertMany<T> : ICallistoOperation<T> where T : class, IDocumentRoot
    {
        IEnumerable<T> Values { get; }
        InsertManyOptions InsertManyOptions { get; }
    }
}