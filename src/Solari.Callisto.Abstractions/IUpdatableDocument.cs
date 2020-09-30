using System.Collections.Generic;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.Contracts.CQR;

namespace Solari.Callisto.Abstractions
{
    public interface IUpdatableDocument<T> where T : class, IDocumentRoot
    {
        Queue<UpdateOneModel<T>> PendingUpdates { get; }
    }
}
