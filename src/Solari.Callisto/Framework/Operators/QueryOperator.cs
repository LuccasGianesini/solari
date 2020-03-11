using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.CQR;
using Solari.Callisto.Abstractions.Exceptions;

namespace Solari.Callisto.Framework.Operators
{
    public sealed class QueryOperator<TEntity> where TEntity : class, IDocumentRoot
    {
        private readonly IMongoCollection<TEntity> _collection;
        private readonly ICallistoOperationFactory _factory;

        public QueryOperator(IMongoCollection<TEntity> collection, ICallistoOperationFactory factory)
        {
            _collection = collection;
            _factory = factory;
        }

        /// <summary>
        /// Returns the first document that matched the predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            using (IAsyncCursor<TEntity> cursor = await _collection.FindAsync(predicate).ConfigureAwait(false))
            {
                return cursor.FirstOrDefault();
            }
        }

        public async Task<bool> Exists(Expression<Func<TEntity, bool>> predicate)
        {
            using (IAsyncCursor<TEntity> result = await _collection.FindAsync(predicate).ConfigureAwait(false))
            {
                return await result.AnyAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Find one document by id.
        /// </summary>
        /// <param name="id">The id</param>
        /// <returns></returns>
        public async Task<TEntity> FindById(ObjectId id) =>
            await Find(_factory.CreateQuery($"query-by-id {nameof(TEntity)}",
                                             Builders<TEntity>.Filter.Eq(a => a.Id, id),
                                             cursor => cursor.FirstOrDefault()));


        /// <summary>
        /// Queries the database
        /// </summary>
        /// <param name="factory">The operation factory</param>
        /// <returns></returns>
        public async Task<TResult> Find<TResult>(Func<ICallistoOperationFactory, ICallistoQuery<TEntity, TResult>> factory)
        {
            return await Find(factory(_factory));
        }

        /// <summary>
        /// Execute a query.
        /// </summary>
        /// <param name="operation">The operation</param>
        /// <typeparam name="TResult">The Result</typeparam>
        /// <returns></returns>
        /// <exception cref="NullCallistoOperationException"></exception>
        public async Task<TResult> Find<TResult>(ICallistoQuery<TEntity, TResult> operation)
        {
            if (operation == null)
                throw new NullCallistoOperationException(CallistoOperationHelper.NullOperationInstanceMessage("query", nameof(ICallistoInsert<TEntity>)));
            operation.ValidateOperation();
            if (operation.UseSessionHandle)
            {
                using (IAsyncCursor<TEntity> cursor = await _collection.FindAsync(operation.ClientSessionHandle,
                                                                                  operation.FilterDefinition,
                                                                                  operation.FindOptions,
                                                                                  operation.CancellationToken)
                                                                       .ConfigureAwait(false))
                {
                    return operation.ResultFunction(cursor);
                }
            }

            using (IAsyncCursor<TEntity> cursor = await _collection.FindAsync(operation.FilterDefinition,
                                                                              operation.FindOptions,
                                                                              operation.CancellationToken)
                                                                   .ConfigureAwait(false))
            {
                return operation.ResultFunction(cursor);
            }
        }

        /// <summary>
        /// Execute an aggregation.
        /// </summary>
        /// <param name="factory">The operation factory</param>
        /// <typeparam name="TProjectionModel">The model for the projection</typeparam>
        /// <typeparam name="TResult">The final result type of the operation</typeparam>
        /// <returns></returns>
        /// <exception cref="NullCallistoOperationException"></exception>
        public async Task<TResult> Aggregate<TProjectionModel, TResult>(
            Func<ICallistoOperationFactory, ICallistoAggregation<TEntity, TProjectionModel, TResult>> factory) =>
            await Aggregate(factory(_factory));
        /// <summary>
        /// Execute an aggregation.
        /// </summary>
        /// <param name="operation">The operation</param>
        /// <typeparam name="TProjectionModel">The model for the projection</typeparam>
        /// <typeparam name="TResult">The final result type of the operation</typeparam>
        /// <returns></returns>
        /// <exception cref="NullCallistoOperationException"></exception>
        public async Task<TResult> Aggregate<TProjectionModel, TResult>(ICallistoAggregation<TEntity, TProjectionModel, TResult> operation)
        {
            if (operation == null)
                throw new NullCallistoOperationException(CallistoOperationHelper.NullOperationInstanceMessage("aggregation", nameof(ICallistoInsert<TEntity>)));
            operation.ValidateOperation();
            if (operation.UseSessionHandle)
            {
                using (IAsyncCursor<TProjectionModel> cursor = await _collection.AggregateAsync(operation.ClientSessionHandle,
                                                                                                operation.PipelineDefinition,
                                                                                                operation.AggregateOptions,
                                                                                                operation.CancellationToken))
                {
                    return operation.ResultFunction(cursor);
                }
            }
            using (IAsyncCursor<TProjectionModel> cursor = await _collection.AggregateAsync(operation.PipelineDefinition,
                                                                                            operation.AggregateOptions,
                                                                                            operation.CancellationToken))
            {
                return operation.ResultFunction(cursor);
            }
        }
    }
}