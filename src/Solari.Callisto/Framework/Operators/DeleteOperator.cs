﻿using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.CQR;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Callisto.Abstractions.Validators;

namespace Solari.Callisto.Framework.Operators
{
    public sealed class DeleteOperator<TEntity> where TEntity : class, IDocumentRoot
    {
        private readonly IMongoCollection<TEntity> _collection;
        private readonly ICallistoDeleteOperationFactory _factory;

        public DeleteOperator(IMongoCollection<TEntity> collection, ICallistoDeleteOperationFactory factory)
        {
            _collection = collection;
            _factory = factory;
        }

        /// <summary>
        ///     Delete one document by id.
        /// </summary>
        /// <param name="id">Document id</param>
        /// <returns></returns>
        public async Task<DeleteResult> OneById(ObjectId id) { return await One(_factory.CreateDeleteById<TEntity>(id)); }

        /// <summary>
        ///     Delete many documents from the collection.
        /// </summary>
        /// <param name="factory">The operation factory</param>
        /// <exception cref="NullCallistoOperationException">When command is null</exception>
        /// <exception cref="NullFilterDefinitionException">When command <see cref="FilterDefinition{TDocument}" /> is null</exception>
        /// <returns>
        ///     <see cref="DeleteResult" />
        /// </returns>
        public async Task<DeleteResult> Many(Func<ICallistoDeleteOperationFactory, ICallistoDelete<TEntity>> factory) { return await Many(factory(_factory)); }

        /// <summary>
        ///     Delete one document from the collection.
        /// </summary>
        /// <param name="factory">The operation factory</param>
        /// <exception cref="NullCallistoOperationException">When command is null</exception>
        /// <exception cref="NullFilterDefinitionException">When command <see cref="FilterDefinition{TDocument}" /> is null</exception>
        /// <returns>
        ///     <see cref="DeleteResult" />
        /// </returns>
        public async Task<DeleteResult> One(Func<ICallistoDeleteOperationFactory, ICallistoDelete<TEntity>> factory) { return await One(factory(_factory)); }

        /// <summary>
        ///     Delete one document from the collection.
        /// </summary>
        /// <param name="operation">
        ///     <see cref="ICallistoDelete{T}" />
        /// </param>
        /// <exception cref="NullCallistoOperationException">When command is null</exception>
        /// <exception cref="NullFilterDefinitionException">When command <see cref="FilterDefinition{TDocument}" /> is null</exception>
        /// <returns>
        ///     <see cref="DeleteResult" />
        /// </returns>
        public async Task<DeleteResult> One(ICallistoDelete<TEntity> operation)
        {
            CallistoOperationHelper.PreExecutionCheck(operation);
            
            if (operation.ClientSessionHandle is null)
                return await _collection.DeleteOneAsync(operation.FilterDefinition,
                                                        operation.DeleteOptions,
                                                        operation.CancellationToken).ConfigureAwait(false);
            return await _collection.DeleteOneAsync(operation.ClientSessionHandle,
                                                    operation.FilterDefinition,
                                                    operation.DeleteOptions,
                                                    operation.CancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        ///     Delete many documents from the collection.
        /// </summary>
        /// <param name="operation">
        ///     <see cref="ICallistoDelete{T}" />
        /// </param>
        /// <exception cref="NullCallistoOperationException">When command is null</exception>
        /// <exception cref="NullFilterDefinitionException">When command <see cref="FilterDefinition{TDocument}" /> is null</exception>
        /// <returns>
        ///     <see cref="DeleteResult" />
        /// </returns>
        public async Task<DeleteResult> Many(ICallistoDelete<TEntity> operation)
        {
            CallistoOperationHelper.PreExecutionCheck(operation);
            
            if (operation.ClientSessionHandle is null)
                return await _collection.DeleteManyAsync(operation.FilterDefinition,
                                                         operation.DeleteOptions,
                                                         operation.CancellationToken).ConfigureAwait(false);
            return await _collection.DeleteManyAsync(operation.ClientSessionHandle,
                                                     operation.FilterDefinition,
                                                     operation.DeleteOptions,
                                                     operation.CancellationToken).ConfigureAwait(false);
        }
    }
}