using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.CQR;
using Solari.Callisto.Abstractions.Exceptions;

namespace Solari.Callisto.Framework.Operators
{
    public sealed class ReplaceOperator<TEntity> where TEntity : class, IDocumentRoot
    {
        private readonly IMongoCollection<TEntity> _collection;
        private readonly ICallistoOperationFactory _factory;

        public ReplaceOperator(IMongoCollection<TEntity> collection, ICallistoOperationFactory factory)
        {
            _collection = collection;
            _factory = factory;
        }

        /// <summary>
        /// Replace one document
        /// </summary>
        /// <param name="factory">Operation factory</param>
        /// <returns></returns>
        public async Task<ReplaceOneResult> One(Func<ICallistoOperationFactory, ICallistoReplace<TEntity>> factory) => await One(factory(_factory));

        /// <summary>
        /// Replace one document by id.
        /// </summary>
        /// <param name="id">The id of the document</param>
        /// <param name="replacement">The replacement entity</param>
        /// <returns></returns>
        public async Task<ReplaceOneResult> OneVyId(ObjectId id, TEntity replacement) =>
            await One(_factory.CreateReplaceById($"replace {nameof(TEntity)}", replacement, id));
        /// <summary>
        /// Replace one document from the collection.
        /// </summary>
        /// <param name="operation"><see cref="ICallistoDelete{T}"/></param>
        /// <exception cref="NullCallistoOperationException">When command is null</exception>
        /// <exception cref="NullFilterDefinitionException">When command <see cref="FilterDefinition{TDocument}"/> is null</exception>
        /// <exception cref="ArgumentNullException">When replacement value is null</exception>
        /// <returns><see cref="DeleteResult"/></returns>
        public async Task<ReplaceOneResult> One(ICallistoReplace<TEntity> operation)
        {
            if (operation == null)
                throw new NullCallistoOperationException(CallistoOperationHelper.NullOperationInstanceMessage("delete-one", nameof(ICallistoInsert<TEntity>)));
            operation.ValidateOperation();
            if (operation.UseSessionHandle)
            {
                return await _collection.ReplaceOneAsync(operation.ClientSessionHandle,
                                                         operation.FilterDefinition,
                                                         operation.Replacement,
                                                         operation.ReplaceOptions,
                                                         operation.CancellationToken).ConfigureAwait(false);
            }

            return await _collection.ReplaceOneAsync(operation.FilterDefinition,
                                                     operation.Replacement,
                                                     operation.ReplaceOptions,
                                                     operation.CancellationToken).ConfigureAwait(false);
        }
    }
}