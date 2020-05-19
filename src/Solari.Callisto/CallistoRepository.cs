using System.Threading.Tasks;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Framework.Operators;

namespace Solari.Callisto
{
    public abstract class CallistoRepository<T> where T : class, IDocumentRoot
    {
        protected CallistoRepository(ICallistoContext<T> context)
        {
            Context = context;
            Collection = context.Collection;
            OperationFactory = context.OperationFactory;
            Insert = new InsertOperator<T>(Collection, OperationFactory);
            Update = new UpdateOperator<T>(Collection, OperationFactory);
            Delete = new DeleteOperator<T>(Collection, OperationFactory);
            Replace = new ReplaceOperator<T>(Collection, OperationFactory);
            Query = new QueryOperator<T>(Collection, OperationFactory);
        }

        protected IMongoCollection<T> Collection { get; }
        protected ICallistoContext<T> Context { get; }
        protected ICallistoOperationFactory OperationFactory { get; }
        protected InsertOperator<T> Insert { get; }

        protected UpdateOperator<T> Update { get; }

        protected DeleteOperator<T> Delete { get; }

        protected ReplaceOperator<T> Replace { get; }

        protected QueryOperator<T> Query { get; }

        protected async Task<IClientSessionHandle> StartSessionAsync() => await Context.Connection.GetClient().StartSessionAsync();
        protected void EndSession(IClientSessionHandle sessionHandle) => sessionHandle.Dispose();
    }
}