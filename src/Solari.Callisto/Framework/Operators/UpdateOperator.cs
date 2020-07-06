using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.CQR;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Callisto.Framework.Factories;

namespace Solari.Callisto.Framework.Operators
{
    public sealed class UpdateOperator<T> where T : class, IDocumentRoot
    {
        private readonly IMongoCollection<T> _collection;
        private readonly ICallistoUpdateOperationFactory _factory;

        public UpdateOperator(IMongoCollection<T> collection, ICallistoUpdateOperationFactory factory)
        {
            _collection = collection;
            _factory = factory;
        }

        /// <summary>
        ///     Update many documents at once.
        /// </summary>
        /// <param name="factory">Command Factory</param>
        /// <exception cref="NullCallistoOperationException">When command is null</exception>
        /// <exception cref="NullUpdateDefinitionException">When command <see cref="UpdateDefinition{TDocument}" /> is null</exception>
        /// <exception cref="NullFilterDefinitionException">When command <see cref="FilterDefinition{TDocument}" /> is null</exception>
        /// <returns>
        ///     <see cref="UpdateResult" />
        /// </returns>
        public async Task<UpdateResult> Many(Func<ICallistoUpdateOperationFactory, ICallistoUpdate<T>> factory) { return await Many(factory(_factory)); }

        /// <summary>
        ///     Update a single document.
        /// </summary>
        /// <param name="factory">Command Factory</param>
        /// <exception cref="NullCallistoOperationException">When command is null</exception>
        /// <exception cref="NullUpdateDefinitionException">When command <see cref="FilterDefinition{TDocument}" /> is null</exception>
        /// <exception cref="NullFilterDefinitionException">When command <see cref="FilterDefinition{TDocument}" /> is null</exception>
        /// <returns>
        ///     <see cref="UpdateResult" />
        /// </returns>
        public async Task<UpdateResult> One(Func<ICallistoUpdateOperationFactory, ICallistoUpdate<T>> factory) { return await One(factory(_factory)); }

        /// <summary>
        ///     Update a single document.
        /// </summary>
        /// <param name="operation">Instance of class implementing <see cref="ICallistoUpdate{T}" /></param>
        /// <exception cref="NullCallistoOperationException">When command is null</exception>
        /// <exception cref="NullUpdateDefinitionException">When command <see cref="FilterDefinition{TDocument}" /> is null</exception>
        /// <exception cref="NullFilterDefinitionException">When command <see cref="FilterDefinition{TDocument}" /> is null</exception>
        /// <returns>
        ///     <see cref="UpdateResult" />
        /// </returns>
        public async Task<UpdateResult> One(ICallistoUpdate<T> operation)
        {
            Helper.PreExecutionCheck(operation);

            if (operation.ClientSessionHandle is null)
                return await _collection.UpdateOneAsync(operation.FilterDefinition,
                                                        operation.UpdateDefinition,
                                                        operation.UpdateOptions,
                                                        operation.CancellationToken).ConfigureAwait(false);

            return await _collection.UpdateOneAsync(operation.ClientSessionHandle,
                                                    operation.FilterDefinition,
                                                    operation.UpdateDefinition,
                                                    operation.UpdateOptions,
                                                    operation.CancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        ///     Update many documents at once.
        /// </summary>
        /// <param name="operation">Instance of class implementing <see cref="ICallistoUpdate{TEntity}" /></param>
        /// <exception cref="NullCallistoOperationException">When command is null</exception>
        /// <exception cref="NullUpdateDefinitionException">When command <see cref="UpdateDefinition{TDocument}" /> is null</exception>
        /// <exception cref="NullFilterDefinitionException">When command <see cref="FilterDefinition{TDocument}" /> is null</exception>
        /// <returns>
        ///     <see cref="UpdateResult" />
        /// </returns>
        public async Task<UpdateResult> Many(ICallistoUpdate<T> operation)
        {
            Helper.PreExecutionCheck(operation);
            if (operation.ClientSessionHandle is null)
                return await _collection.UpdateManyAsync(operation.FilterDefinition,
                                                         operation.UpdateDefinition,
                                                         operation.UpdateOptions,
                                                         operation.CancellationToken).ConfigureAwait(false);

            return await _collection.UpdateManyAsync(operation.ClientSessionHandle,
                                                     operation.FilterDefinition,
                                                     operation.UpdateDefinition,
                                                     operation.UpdateOptions,
                                                     operation.CancellationToken).ConfigureAwait(false);
        }
    }
}
