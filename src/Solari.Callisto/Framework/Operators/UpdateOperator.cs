using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.CQR;
using Solari.Callisto.Abstractions.Exceptions;

namespace Solari.Callisto.Framework.Operators
{
    public sealed class UpdateOperator<TEntity> where TEntity : class, IDocumentRoot
    {
        private readonly IMongoCollection<TEntity> _collection;
        private readonly ICallistoOperationFactory _factory;

        public UpdateOperator(IMongoCollection<TEntity> collection, ICallistoOperationFactory factory)
        {
            _collection = collection;
            _factory = factory;
        }

        /// <summary>
        /// Update many documents at once.
        /// </summary>
        /// <param name="factory">Command Factory</param>
        /// <exception cref="NullCallistoOperationException">When command is null</exception>
        /// <exception cref="NullUpdateDefinitionException">When command <see cref="UpdateDefinition{TDocument}"/> is null</exception>
        /// <exception cref="NullFilterDefinitionException">When command <see cref="FilterDefinition{TDocument}"/> is null</exception>
        /// <returns><see cref="UpdateResult"/></returns>
        public async Task<UpdateResult> Many(Func<ICallistoOperationFactory, ICallistoUpdate<TEntity>> factory) => await Many(factory(_factory));

        /// <summary>
        /// Update a single document.
        /// </summary>
        /// <param name="factory">Command Factory</param>
        /// <exception cref="NullCallistoOperationException">When command is null</exception>
        /// <exception cref="NullUpdateDefinitionException">When command <see cref="FilterDefinition{TDocument}"/> is null</exception>
        /// <exception cref="NullFilterDefinitionException">When command <see cref="FilterDefinition{TDocument}"/> is null</exception>
        /// <returns><see cref="UpdateResult"/></returns>
        public async Task<UpdateResult> One(Func<ICallistoOperationFactory, ICallistoUpdate<TEntity>> factory) => await One(factory(_factory));

        /// <summary>
        /// Update a single document.
        /// </summary>
        /// <param name="operation">Instance of class implementing <see cref="ICallistoUpdate{T}"/></param>
        /// <exception cref="NullCallistoOperationException">When command is null</exception>
        /// <exception cref="NullUpdateDefinitionException">When command <see cref="FilterDefinition{TDocument}"/> is null</exception>
        /// <exception cref="NullFilterDefinitionException">When command <see cref="FilterDefinition{TDocument}"/> is null</exception>
        /// <returns><see cref="UpdateResult"/></returns>
        public async Task<UpdateResult> One(ICallistoUpdate<TEntity> operation)
        {
            if (operation == null)
                throw new NullCallistoOperationException(CallistoOperationHelper.NullOperationInstanceMessage("update-one", nameof(ICallistoUpdate<TEntity>)));
            operation.ValidateOperation();

            if (operation.UseSessionHandle)
            {
                return await _collection.UpdateOneAsync(operation.ClientSessionHandle,
                                                        operation.FilterDefinition,
                                                        operation.UpdateDefinition,
                                                        operation.UpdateOptions,
                                                        operation.CancellationToken).ConfigureAwait(false);
            }

            return await _collection.UpdateOneAsync(operation.FilterDefinition,
                                                    operation.UpdateDefinition,
                                                    operation.UpdateOptions,
                                                    operation.CancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Update many documents at once.
        /// </summary>
        /// <param name="operation">Instance of class implementing <see cref="ICallistoUpdate{TEntity}"/></param>
        /// <exception cref="NullCallistoOperationException">When command is null</exception>
        /// <exception cref="NullUpdateDefinitionException">When command <see cref="UpdateDefinition{TDocument}"/> is null</exception>
        /// <exception cref="NullFilterDefinitionException">When command <see cref="FilterDefinition{TDocument}"/> is null</exception>
        /// <returns><see cref="UpdateResult"/></returns>
        public async Task<UpdateResult> Many(ICallistoUpdate<TEntity> operation)
        {
            if (operation.UseSessionHandle)
            {
                return await _collection.UpdateManyAsync(operation.ClientSessionHandle,
                                                         operation.FilterDefinition,
                                                         operation.UpdateDefinition,
                                                         operation.UpdateOptions,
                                                         operation.CancellationToken).ConfigureAwait(false);
            }

            return await _collection.UpdateManyAsync(operation.FilterDefinition,
                                                     operation.UpdateDefinition,
                                                     operation.UpdateOptions,
                                                     operation.CancellationToken).ConfigureAwait(false);
        }
    }
}