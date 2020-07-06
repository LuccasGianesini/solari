using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Framework.Factories;

namespace Solari.Callisto.Framework.Operators
{
    public sealed class DeleteOperator<T> where T : class, IDocumentRoot
    {
        private readonly IMongoCollection<T> _collection;
        private readonly ICallistoDeleteOperationFactory _factory;

        public DeleteOperator(IMongoCollection<T> collection, ICallistoDeleteOperationFactory factory)
        {
            _collection = collection;
            _factory = factory;
        }

        /// <summary>
        ///     Delete one document by id.
        /// </summary>
        /// <param name="id">Document id</param>
        /// <returns></returns>
        public async Task<DeleteResult> OneById(Guid id) { return await One(_factory.CreateDeleteById<T>(id)); }

        /// <summary>
        ///     Delete many documents from the collection.
        /// </summary>
        /// <param name="factory">The operation factory</param>
        /// <exception cref="NullCallistoOperationException">When command is null</exception>
        /// <exception cref="NullFilterDefinitionException">When command <see cref="FilterDefinition{TDocument}" /> is null</exception>
        /// <returns>
        ///     <see cref="DeleteResult" />
        /// </returns>
        public async Task<DeleteResult> Many(Func<ICallistoDeleteOperationFactory, ICallistoDelete<T>> factory) { return await Many(factory(_factory)); }

        /// <summary>
        ///     Delete one document from the collection.
        /// </summary>
        /// <param name="factory">The operation factory</param>
        /// <exception cref="NullCallistoOperationException">When command is null</exception>
        /// <exception cref="NullFilterDefinitionException">When command <see cref="FilterDefinition{TDocument}" /> is null</exception>
        /// <returns>
        ///     <see cref="DeleteResult" />
        /// </returns>
        public async Task<DeleteResult> One(Func<ICallistoDeleteOperationFactory, ICallistoDelete<T>> factory) { return await One(factory(_factory)); }

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
        public async Task<DeleteResult> One(ICallistoDelete<T> operation)
        {
            Helper.PreExecutionCheck(operation);

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
        public async Task<DeleteResult> Many(ICallistoDelete<T> operation)
        {
            Helper.PreExecutionCheck(operation);

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
