using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.CQR;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Callisto.Framework.Factories;

namespace Solari.Callisto.Framework.Operators
{
    public sealed class ReplaceOperator<T> where T : class, IDocumentRoot
    {
        private readonly IMongoCollection<T> _collection;
        private readonly ICallistoReplaceOperationFactory _factory;

        public ReplaceOperator(IMongoCollection<T> collection, ICallistoReplaceOperationFactory factory)
        {
            _collection = collection;
            _factory = factory;
        }

        /// <summary>
        ///     Replace one document
        /// </summary>
        /// <param name="factory">Operation factory</param>
        /// <returns></returns>
        public async Task<ReplaceOneResult> One(Func<ICallistoReplaceOperationFactory, ICallistoReplace<T>> factory) { return await One(factory(_factory)); }

        /// <summary>
        ///     Replace one document by id.
        /// </summary>
        /// <param name="id">The id of the document</param>
        /// <param name="replacement">The replacement entity</param>
        /// <returns></returns>
        public async Task<ReplaceOneResult> OneVyId(Guid id, T replacement) { return await One(_factory.CreateReplaceById(replacement, id)); }

        /// <summary>
        ///     Replace one document from the collection.
        /// </summary>
        /// <param name="operation">
        ///     <see cref="ICallistoDelete{T}" />
        /// </param>
        /// <exception cref="NullCallistoOperationException">When command is null</exception>
        /// <exception cref="NullFilterDefinitionException">When command <see cref="FilterDefinition{TDocument}" /> is null</exception>
        /// <exception cref="ArgumentNullException">When replacement value is null</exception>
        /// <returns>
        ///     <see cref="DeleteResult" />
        /// </returns>
        public async Task<ReplaceOneResult> One(ICallistoReplace<T> operation)
        {
            Helper.PreExecutionCheck(operation);
            if (operation.ClientSessionHandle is null)
                return await _collection.ReplaceOneAsync(operation.FilterDefinition,
                                                         operation.Replacement,
                                                         operation.ReplaceOptions,
                                                         operation.CancellationToken).ConfigureAwait(false);
            return await _collection.ReplaceOneAsync(operation.ClientSessionHandle,
                                                     operation.FilterDefinition,
                                                     operation.Replacement,
                                                     operation.ReplaceOptions,
                                                     operation.CancellationToken).ConfigureAwait(false);
        }
    }
}
