using System;
using System.Threading.Tasks;
using k8s.KubeConfigModels;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.CQR;
using Solari.Callisto.Framework.Operators;

namespace Solari.Callisto
{
    public abstract class CallistoRepository<T> where T : class, IDocumentRoot
    {
        protected CallistoRepository(ICallistoCollectionContext<T> collectionContext)
        {
            CollectionContext = collectionContext;
            Collection = collectionContext.Collection;

        }
        protected IMongoCollection<T> Collection { get; }
        protected ICallistoCollectionContext<T> CollectionContext { get; }

        protected InsertOperator<T> Insert => CollectionContext.Operators.InsertOperator;
        protected UpdateOperator<T> Update => CollectionContext.Operators.UpdateOperator;
        protected DeleteOperator<T> Delete => CollectionContext.Operators.DeleteOperator;
        protected ReplaceOperator<T> Replace => CollectionContext.Operators.ReplaceOperator;
        protected QueryOperator<T> Query => CollectionContext.Operators.QueryOperator;

        protected async Task<IClientSessionHandle> StartSessionAsync() => await CollectionContext.Connection.GetClient().StartSessionAsync();

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
