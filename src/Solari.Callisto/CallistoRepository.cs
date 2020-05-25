using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.CQR;
using Solari.Callisto.Framework.Operators;

namespace Solari.Callisto
{
    public abstract class CallistoRepository<T> where T : class, IDocumentRoot
    {
        private readonly Lazy<InsertOperator<T>> _insertOperator;
        private readonly Lazy<UpdateOperator<T>> _updateOperator;
        private readonly Lazy<ReplaceOperator<T>> _replaceOperator;
        private readonly Lazy<DeleteOperator<T>> _deleteOperator;
        private readonly Lazy<QueryOperator<T>> _queryOperator;

        protected CallistoRepository(ICallistoContext<T> context)
        {
            Context = context;
            Collection = context.Collection;
            _insertOperator = new Lazy<InsertOperator<T>>(() => new InsertOperator<T>(context.Collection, context.InsertOperationFactory));
            _updateOperator = new Lazy<UpdateOperator<T>>(() => new UpdateOperator<T>(context.Collection, context.UpdateOperationFactory));
            _queryOperator = new Lazy<QueryOperator<T>>(() => new QueryOperator<T>(context.Collection, context.QueryOperationFactory));
            _replaceOperator = new Lazy<ReplaceOperator<T>>(() => new ReplaceOperator<T>(context.Collection, context.ReplaceOperationFactory));
            _deleteOperator = new Lazy<DeleteOperator<T>>(() => new DeleteOperator<T>(context.Collection, context.DeleteOperationFactory));
        }

        protected IMongoCollection<T> Collection { get; }
        protected ICallistoContext<T> Context { get; }

        protected InsertOperator<T> Insert => _insertOperator.Value;
        protected UpdateOperator<T> Update => _updateOperator.Value;

        protected DeleteOperator<T> Delete => _deleteOperator.Value;

        protected ReplaceOperator<T> Replace => _replaceOperator.Value;

        protected QueryOperator<T> Query => _queryOperator.Value;

        protected async Task<IClientSessionHandle> StartSessionAsync() => await Context.Connection.GetClient().StartSessionAsync();

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