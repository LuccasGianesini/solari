﻿using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Framework.Operators;

namespace Solari.Callisto
{
    public abstract class CallistoRepository<TEntity> where TEntity : class, IDocumentRoot
    {
        protected CallistoRepository(ICallistoContext context)
        {
            Context = context;
            Collection = Context.Connection.GetDataBase().GetCollection<TEntity>(context.CollectionName);
            OperationFactory = context.OperationFactory;
            Insert = new InsertOperator<TEntity>(Collection, OperationFactory);
            Update = new UpdateOperator<TEntity>(Collection, OperationFactory);
            Delete = new DeleteOperator<TEntity>(Collection, OperationFactory);
            Replace = new ReplaceOperator<TEntity>(Collection, OperationFactory);
            Query = new QueryOperator<TEntity>(Collection, OperationFactory);
        }

        protected IMongoCollection<TEntity> Collection { get; }
        protected ICallistoContext Context { get; }
        protected ICallistoOperationFactory OperationFactory { get; }
        protected InsertOperator<TEntity> Insert { get; }

        protected UpdateOperator<TEntity> Update { get; }

        protected DeleteOperator<TEntity> Delete { get; }

        protected ReplaceOperator<TEntity> Replace { get; }

        protected QueryOperator<TEntity> Query { get; }
    }
}