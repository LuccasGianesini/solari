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
        private readonly ICallistoOperationFactory _factory;

        public InsertOperator(IMongoCollection<TEntity> collection, ICallistoOperationFactory factory)
        {
            _collection = collection;
            _factory = factory;
        }

        /// <summary>
        /// Insert one document into the collection.
        /// </summary>
        /// <param name="factory">The operation factory</param>
        /// <returns>The inserted entities with id</returns>
        /// <exception cref="NullCallistoOperationException">When <see cref="ICallistoInsert{T}"/> is null</exception>
        /// <exception cref="NullOrEmptyValueException">When the values array is null or empty</exception>
        public async Task<TEntity> One(Func<ICallistoOperationFactory, ICallistoInsert<TEntity>> factory) => await One(factory(_factory));
        /// <summary>
        /// Insert many documents into the collection.
        /// </summary>
        /// <param name="factory">The operation factory</param>
        /// <returns>The inserted entities with id</returns>
        /// <exception cref="NullCallistoOperationException">When <see cref="ICallistoInsertMany{T}"/> is null</exception>
        /// <exception cref="NullOrEmptyValueException">When the values array is null or empty</exception>
        public async Task<IEnumerable<TEntity>> Many(Func<ICallistoOperationFactory, ICallistoInsertMany<TEntity>> factory) => await Many(factory(_factory));
        /// <summary>
        /// Insert one document into the collection.
        /// </summary>
        /// <param name="entity">Entity to be inserted</param>
        /// <returns>The inserted entities with id</returns>
        /// <exception cref="NullCallistoOperationException">When <see cref="ICallistoInsert{T}"/> is null</exception>
        /// <exception cref="NullOrEmptyValueException">When the values array is null or empty</exception>
        /// 
        public async Task<TEntity> One(TEntity entity) { return await One(_factory.CreateInsert($"insert {nameof(TEntity)}", entity)); }

        /// <summary>
        /// Insert one document into the collection.
        /// </summary>
        /// <param name="operation"><see cref="ICallistoInsertMany{T}"/></param>
        /// <returns>The inserted entities with id</returns>
        /// <exception cref="NullCallistoOperationException">When <see cref="ICallistoInsert{T}"/> is null</exception>
        /// <exception cref="NullOrEmptyValueException">When the values array is null or empty</exception>
        public async Task<TEntity> One(ICallistoInsert<TEntity> operation)
        {
            if (operation == null)
                throw new NullCallistoOperationException(CallistoOperationHelper.NullOperationInstanceMessage("insert-one", nameof(ICallistoInsert<TEntity>)));
            operation.ValidateOperation();
            if (operation.UseSessionHandle)
            {
                await _collection.InsertOneAsync(operation.ClientSessionHandle,
                                                 operation.Value,
                                                 operation.InsertOneOptions,
                                                 operation.CancellationToken)
                                 .ConfigureAwait(false);
                return operation.Value;
            }

            await _collection.InsertOneAsync(operation.Value,
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
        public async Task<IEnumerable<TEntity>> Many(IEnumerable<TEntity> entities)
        {
            return await Many(_factory.CreateInsertMany($"insert-many {nameof(TEntity)}", entities));
        }

        /// <summary>
        /// Insert many documents into the collection.
        /// </summary>
        /// <param name="operation"><see cref="ICallistoInsertMany{T}"/></param>
        /// <returns>The inserted entities with id</returns>
        /// <exception cref="NullCallistoOperationException">When <see cref="ICallistoInsertMany{T}"/> is null</exception>
        /// <exception cref="NullOrEmptyValueException">When the values array is null or empty</exception>
        public async Task<IEnumerable<TEntity>> Many(ICallistoInsertMany<TEntity> operation)
        {
            if (operation == null)
                throw new NullCallistoOperationException(CallistoOperationHelper.NullOperationInstanceMessage("insert-many",
                                                                                                              nameof(ICallistoInsertMany<TEntity>)));
            operation.ValidateOperation();
            if (operation.UseSessionHandle)
            {
                await _collection.InsertManyAsync(operation.ClientSessionHandle,
                                                  operation.Values,
                                                  operation.InsertManyOptions,
                                                  operation.CancellationToken);
                return operation.Values;
            }

            await _collection.InsertManyAsync(operation.Values,
                                              operation.InsertManyOptions,
                                              operation.CancellationToken);
            return operation.Values;
        }
    }
}