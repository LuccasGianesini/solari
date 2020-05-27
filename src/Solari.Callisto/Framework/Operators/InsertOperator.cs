using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.CQR;
using Solari.Callisto.Abstractions.Exceptions;

namespace Solari.Callisto.Framework.Operators
{
    public sealed class InsertOperator<TEntity> where TEntity : class, IDocumentRoot
    {
        private readonly IMongoCollection<TEntity> _collection;
        private readonly ICallistoInsertOperationFactory _factory;

        public InsertOperator(IMongoCollection<TEntity> collection, ICallistoInsertOperationFactory factory)
        {
            _collection = collection;
            _factory = factory;
        }

        /// <summary>
        /// Insert one document into the collection.
        /// </summary>
        /// <param name="factory">The operation factory</param>
        /// <returns>The inserted entities with id</returns>
        /// <exception cref="NullCallistoOperationException">When <see cref="ICallistoInsertOne{T}"/> is null</exception>
        /// <exception cref="NullOrEmptyValueException">When the values array is null or empty</exception>
        public async Task<TEntity> One(Func<ICallistoInsertOperationFactory, ICallistoInsertOne<TEntity>> factory) { return await One(factory(_factory)); }

        /// <summary>
        /// Insert many documents into the collection.
        /// </summary>
        /// <param name="factory">The operation factory</param>
        /// <returns>The inserted entities with id</returns>
        /// <exception cref="NullCallistoOperationException">When <see cref="ICallistoInsertMany{T}"/> is null</exception>
        /// <exception cref="NullOrEmptyValueException">When the values array is null or empty</exception>
        public async Task<IEnumerable<TEntity>> Many(Func<ICallistoInsertOperationFactory, ICallistoInsertMany<TEntity>> factory)
        {
            return await Many(factory(_factory));
        }

        /// <summary>
        /// Insert one document into the collection.
        /// </summary>
        /// <param name="entity">Entity to be inserted</param>
        /// <returns>The inserted entities with id</returns>
        /// <exception cref="NullCallistoOperationException">When <see cref="ICallistoInsertOne{T}"/> is null</exception>
        /// <exception cref="NullOrEmptyValueException">When the values array is null or empty</exception>
        /// 
        public async Task<TEntity> One(TEntity entity) { return await One(_factory.CreateInsertOne(entity)); }

        /// <summary>
        /// Insert one document into the collection.
        /// </summary>
        /// <param name="operation"><see cref="ICallistoInsertMany{T}"/></param>
        /// <returns>The inserted entities with id</returns>
        /// <exception cref="NullCallistoOperationException">When <see cref="ICallistoInsertOne{T}"/> is null</exception>
        /// <exception cref="NullOrEmptyValueException">When the values array is null or empty</exception>
        public async Task<TEntity> One(ICallistoInsertOne<TEntity> operation)
        {
            CallistoOperationHelper.PreExecutionCheck(operation);
            if (operation.ClientSessionHandle is null)
            {
                await _collection.InsertOneAsync(operation.Value,
                                                 operation.InsertOneOptions,
                                                 operation.CancellationToken)
                                 .ConfigureAwait(false);

                return operation.Value;
            }

            await _collection.InsertOneAsync(operation.ClientSessionHandle,
                                             operation.Value,
                                             operation.InsertOneOptions,
                                             operation.CancellationToken)
                             .ConfigureAwait(false);
            return operation.Value;
        }


        /// <summary>
        /// Insert many documents into the collection.
        /// </summary>
        /// <param name="entities">Entities to be inserted</param>
        /// <returns>The inserted entities with id</returns>
        /// <exception cref="NullCallistoOperationException">When <see cref="ICallistoInsertMany{T}"/> is null</exception>
        /// <exception cref="NullOrEmptyValueException">When the values array is null or empty</exception>
        public async Task<IEnumerable<TEntity>> Many(IEnumerable<TEntity> entities) { return await Many(_factory.CreateInsertMany(entities)); }

        /// <summary>
        /// Insert many documents into the collection.
        /// </summary>
        /// <param name="operation"><see cref="ICallistoInsertMany{T}"/></param>
        /// <returns>The inserted entities with id</returns>
        /// <exception cref="NullCallistoOperationException">When <see cref="ICallistoInsertMany{T}"/> is null</exception>
        /// <exception cref="NullOrEmptyValueException">When the values array is null or empty</exception>
        public async Task<IEnumerable<TEntity>> Many(ICallistoInsertMany<TEntity> operation)
        {
            CallistoOperationHelper.PreExecutionCheck(operation);

            if (operation.ClientSessionHandle is null)
            {
                await _collection.InsertManyAsync(operation.Values,
                                                  operation.InsertManyOptions,
                                                  operation.CancellationToken);
                return operation.Values;
            }

            await _collection.InsertManyAsync(operation.ClientSessionHandle,
                                              operation.Values,
                                              operation.InsertManyOptions,
                                              operation.CancellationToken);
            return operation.Values;
        }
    }
}