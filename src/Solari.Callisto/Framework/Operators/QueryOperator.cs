using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Framework.Factories;

namespace Solari.Callisto.Framework.Operators
{
    public sealed class QueryOperator<T> where T : class, IDocumentRoot
    {
        private readonly IMongoCollection<T> _collection;
        private readonly ICallistoQueryOperationFactory _factory;

        public QueryOperator(IMongoCollection<T> collection, ICallistoQueryOperationFactory factory)
        {
            _collection = collection;
            _factory = factory;
        }

        /// <summary>
        ///     Returns the first document that matched the predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<T> Find(Expression<Func<T, bool>> predicate)
        {
            using (IAsyncCursor<T> cursor = await _collection.FindAsync(predicate).ConfigureAwait(false))
            {
                return cursor.FirstOrDefault();
            }
        }

        public async Task<bool> Exists(Expression<Func<T, bool>> predicate)
        {
            using (IAsyncCursor<T> result = await _collection.FindAsync(predicate).ConfigureAwait(false))
            {
                return await result.AnyAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        ///     Find one document by id.
        /// </summary>
        /// <param name="id">The id</param>
        /// <returns></returns>
        public async Task<T> FindById(Guid id)
        {
            return await Find(_factory.CreateExecutableQuery(Builders<T>.Filter.Eq(a => a.Id, id),
                                                   cursor => cursor.FirstOrDefault()));
        }


        /// <summary>
        ///     Queries the database
        /// </summary>
        /// <param name="factory">The operation factory</param>
        /// <returns></returns>
        public async Task<TResult> Find<TResult>(Func<ICallistoQueryOperationFactory, ICallistoExecutableQuery<T, TResult>> factory)
        {
            return await Find(factory(_factory));
        }

        /// <summary>
        ///     Execute a query.
        /// </summary>
        /// <param name="operation">The operation</param>
        /// <typeparam name="TResult">The Result</typeparam>
        /// <returns></returns>
        public async Task<TResult> Find<TResult>(ICallistoExecutableQuery<T, TResult> operation)
        {
            Helper.PreExecutionCheck(operation);
            if (operation.ClientSessionHandle is null)
                using (IAsyncCursor<T> cursor = await _collection.FindAsync(operation.FilterDefinition,
                                                                            operation.FindOptions,
                                                                            operation.CancellationToken)
                                                                 .ConfigureAwait(false))
                {
                    return operation.ResultFunction(cursor);
                }

            using (IAsyncCursor<T> cursor = await _collection.FindAsync(operation.ClientSessionHandle,
                                                                        operation.FilterDefinition,
                                                                        operation.FindOptions,
                                                                        operation.CancellationToken)
                                                             .ConfigureAwait(false))
            {
                return operation.ResultFunction(cursor);
            }
        }

        /// <summary>
        ///     Execute an aggregation.
        /// </summary>
        /// <param name="factory">The operation factory</param>
        /// <typeparam name="TProjectionModel">The model for the projection</typeparam>
        /// <typeparam name="TResult">The final result type of the operation</typeparam>
        /// <returns></returns>
        /// <exception cref="NullCallistoOperationException"></exception>
        public async Task<TResult> Aggregate<TProjectionModel, TResult>(Func<ICallistoQueryOperationFactory, ICallistoAggregation<T, TProjectionModel, TResult>> factory)
            where TProjectionModel : class
        {
            return await Aggregate(factory(_factory));
        }

        /// <summary>
        ///     Execute an aggregation.
        /// </summary>
        /// <param name="operation">The operation</param>
        /// <typeparam name="TProjectionModel">The model for the projection</typeparam>
        /// <typeparam name="TResult">The final result type of the operation</typeparam>
        /// <returns></returns>
        /// <exception cref="NullCallistoOperationException"></exception>
        public async Task<TResult> Aggregate<TProjectionModel, TResult>(ICallistoAggregation<T, TProjectionModel, TResult> operation)
            where TProjectionModel : class
        {
            Helper.PreExecutionCheck(operation);
            if (operation.ClientSessionHandle is null)
                using (IAsyncCursor<TProjectionModel> cursor = await _collection.AggregateAsync(operation.PipelineDefinition,
                                                                                                operation.AggregateOptions,
                                                                                                operation.CancellationToken))
                {
                    return operation.ResultFunction(cursor);
                }

            using (IAsyncCursor<TProjectionModel> cursor = await _collection.AggregateAsync(operation.ClientSessionHandle,
                                                                                            operation.PipelineDefinition,
                                                                                            operation.AggregateOptions,
                                                                                            operation.CancellationToken))
            {
                return operation.ResultFunction(cursor);
            }
        }

        public async Task<IAsyncCursor<T>> Query(ICallistoQuery<T> operation)
        {
            Helper.PreExecutionCheck(operation);
            if (operation.ClientSessionHandle is null)
                return await _collection.FindAsync(operation.FilterDefinition, operation.FindOptions);
            return await _collection.FindAsync(operation.ClientSessionHandle, operation.FilterDefinition, operation.FindOptions);
        }
    }
}
