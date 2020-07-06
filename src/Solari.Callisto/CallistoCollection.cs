using System.Threading.Tasks;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.CQR;
using Solari.Callisto.Framework;

namespace Solari.Callisto
{
    public abstract class CallistoCollection<T> where T : class, IDocumentRoot
    {
        protected CallistoCollection(ICallistoCollectionContext<T> collectionContext, ICollectionOperators<T> operators)
        {
            CollectionContext = collectionContext;
            Operators = operators;
            Collection = collectionContext.Collection;
        }

        protected IMongoCollection<T> Collection { get; }
        protected ICallistoCollectionContext<T> CollectionContext { get; }
        protected ICollectionOperators<T> Operators { get; }
        protected async Task<IClientSessionHandle> StartSessionAsync()
            => await CollectionContext.CallistoClient.MongoClient.StartSessionAsync();

        protected async Task<IClientSessionHandle> StartSessionAsync(ICallistoOperation<T> operation)
        {
            IClientSessionHandle session = await StartSessionAsync();
            operation.UseSessionHandle(session);
            return session;
        }

        protected void EndSession(ICallistoOperation<T> operation) => EndSession(operation.ClientSessionHandle);
        protected void EndSession(IClientSessionHandle sessionHandle) => sessionHandle.Dispose();
    }
}
