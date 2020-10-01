using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Sol;

namespace Solari.Callisto
{
    public static class CallistoCollectionContextExtensions
    {
        /// <summary>
        /// Insert a collection of documents into the mongodb collection.
        /// </summary>
        /// <param name="context">Mongodb collection context</param>
        /// <param name="documents">Collection of documents to be inserted</param>
        /// <param name="options"><see cref="BulkWriteOptions"/></param>
        /// <typeparam name="TDocument">Collection type</typeparam>
        /// <returns><see cref="BulkWriteResult{T}"/></returns>
        public static async ValueTask<BulkWriteResult<TDocument>> BulkInsert<TDocument>(this ICallistoCollectionContext<TDocument> context, IEnumerable<TDocument> documents,
                                                                                        BulkWriteOptions options = null)
            where TDocument : class, IDocumentRoot, IInsertableDocument
        {
            IEnumerable<TDocument> updatableDocuments = documents.ToList();
            ValidateOperationParameters(updatableDocuments, context, "bulk-insert");
            return await Write(context, documents.Select(a => new InsertOneModel<TDocument>(a)).ToList(), options);
        }

        /// <summary>
        /// Insert a collection of documents into the mongodb collection.
        /// </summary>
        /// <param name="context">Mongodb collection context</param>
        /// <param name="documents">Collection of documents to be inserted</param>
        /// <param name="handle"><see cref="IClientSessionHandle"/></param>
        /// <param name="options"><see cref="BulkWriteOptions"/></param>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <returns><see cref="BulkWriteResult{T}"/></returns>
        public static async Task<BulkWriteResult<TDocument>> BulkInsert<TDocument>(this ICallistoCollectionContext<TDocument> context,
                                                                                   IEnumerable<TDocument> documents,
                                                                                   IClientSessionHandle handle,
                                                                                   BulkWriteOptions options = null)
            where TDocument : class, IDocumentRoot, IInsertableDocument
        {
            IEnumerable<TDocument> updatableDocuments = documents.ToList();
            ValidateOperationParameters(updatableDocuments, context, "bulk-insert");
            return await WriteWithHandle(context, documents.Select(a => new InsertOneModel<TDocument>(a)).ToList(), handle, options);
        }

        /// <summary>
        /// Replace a collection of documents from the mongodb collection.
        /// <remarks>
        ///    The transaction filter is created using the property Id from <see cref="IDocumentRoot"/>. This behaviour is by design.
        /// </remarks>
        /// </summary>
        /// <param name="context">Mongodb collection context</param>
        /// <param name="documents">Collection of documents</param>
        /// <param name="options"><see cref="BulkWriteOptions"/></param>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <returns><see cref="BulkWriteResult{T}"/></returns>
        public static async ValueTask<BulkWriteResult<TDocument>> BulkReplace<TDocument>(this ICallistoCollectionContext<TDocument> context, IEnumerable<TDocument> documents,
                                                                                         BulkWriteOptions options = null)
            where TDocument : class, IDocumentRoot, IReplaceableDocument
        {
            IEnumerable<TDocument> updatableDocuments = documents.ToList();
            ValidateOperationParameters(updatableDocuments, context, "bulk-replace");
            return await Write(context, documents.Select(a => new ReplaceOneModel<TDocument>(a.FilterById(), a)).ToList(), options);
        }

        /// <summary>
        /// Replace a collection of documents from the mongodb collection.
        /// <remarks>
        ///    The transaction filter is created using the property Id from <see cref="IDocumentRoot"/>. This behaviour is by design.
        /// </remarks>
        /// </summary>
        /// <param name="context">Mongodb collection context</param>
        /// <param name="documents">Collection of documents</param>
        /// <param name="handle"><see cref="IClientSessionHandle"/></param>
        /// <param name="options"><see cref="BulkWriteOptions"/></param>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <returns><see cref="BulkWriteResult{T}"/></returns>
        public static async ValueTask<BulkWriteResult<TDocument>> BulkReplace<TDocument>(this ICallistoCollectionContext<TDocument> context, IEnumerable<TDocument> documents,
                                                                                         IClientSessionHandle handle,
                                                                                         BulkWriteOptions options = null)
            where TDocument : class, IDocumentRoot, IReplaceableDocument
        {
            IEnumerable<TDocument> updatableDocuments = documents.ToList();
            ValidateOperationParameters(updatableDocuments, context, "bulk-replace");
            return await WriteWithHandle(context, documents.Select(a => new ReplaceOneModel<TDocument>(a.FilterById(), a)).ToList(), handle, options);
        }

        /// <summary>
        /// Delete a collection of documents from the mongodb collection.
        /// <remarks>
        ///    The transaction filter is created using the property Id from <see cref="IDocumentRoot"/>. This behaviour is by design.
        /// </remarks>
        /// </summary>
        /// <param name="context">Mongodb collection context</param>
        /// <param name="documents">Collection of documents</param>
        /// <param name="handle"><see cref="IClientSessionHandle"/></param>
        /// <param name="options"><see cref="BulkWriteOptions"/></param>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <returns><see cref="BulkWriteResult{T}"/></returns>
        public static async ValueTask<BulkWriteResult<TDocument>> BulkDelete<TDocument>(this ICallistoCollectionContext<TDocument> context, IEnumerable<TDocument> documents,
                                                                                        IClientSessionHandle handle, BulkWriteOptions options = null)
            where TDocument : class, IDocumentRoot, IDeletableDocument
        {
            IEnumerable<TDocument> updatableDocuments = documents.ToList();
            ValidateOperationParameters(updatableDocuments, context, "bulk-delete");
            return await WriteWithHandle(context, documents.Select(a => new DeleteOneModel<TDocument>(a.FilterById())).ToList(), handle, options);
        }

        /// <summary>
        /// Delete a collection of documents from the mongodb collection.
        /// <remarks>
        ///    The transaction filter is created using the property Id from <see cref="IDocumentRoot"/>. This behaviour is by design.
        /// </remarks>
        /// </summary>
        /// <param name="context">Mongodb collection context</param>
        /// <param name="documents">Collection of documents</param>
        /// <param name="options"><see cref="BulkWriteOptions"/></param>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <returns><see cref="BulkWriteResult{T}"/></returns>
        public static async ValueTask<BulkWriteResult<TDocument>> BulkDelete<TDocument>(this ICallistoCollectionContext<TDocument> context, IEnumerable<TDocument> documents,
                                                                                        BulkWriteOptions options = null)
            where TDocument : class, IDocumentRoot, IDeletableDocument
        {
            IEnumerable<TDocument> updatableDocuments = documents.ToList();
            ValidateOperationParameters(updatableDocuments, context, "bulk-delete");
            return await Write(context, documents.Select(a => new DeleteOneModel<TDocument>(a.FilterById())).ToList(), options);
        }

        /// <summary>
        /// Update a collection of documents from the mongodb collection.
        /// </summary>
        /// <param name="context">Mongodb collection context</param>
        /// <param name="documents">Collection of documents</param>
        /// <param name="handle"><see cref="IClientSessionHandle"/></param>
        /// <param name="options"><see cref="BulkWriteOptions"/></param>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <returns><see cref="BulkWriteResult{T}"/></returns>
        public static async ValueTask<BulkWriteResult<TDocument>> BulkUpdate<TDocument>(this ICallistoCollectionContext<TDocument> context, IEnumerable<TDocument> documents,
                                                                                        IClientSessionHandle handle, BulkWriteOptions options = null)
            where TDocument : class, IDocumentRoot, IUpdatableDocument<TDocument>
        {
            IEnumerable<TDocument> updatableDocuments = documents.ToList();
            ValidateOperationParameters(updatableDocuments, context, "bulk-update");
            return await WriteWithHandle(context, documents.SelectMany(a => a.PendingUpdates, (root, model) => model).ToList(), handle, options);
        }

        /// <summary>
        /// Update a collection of documents from the mongodb collection.
        /// </summary>
        /// <param name="context">Mongodb collection context</param>
        /// <param name="documents">Collection of documents</param>
        /// <param name="options"><see cref="BulkWriteOptions"/></param>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <returns><see cref="BulkWriteResult{T}"/></returns>
        public static async ValueTask<BulkWriteResult<TDocument>> BulkUpdate<TDocument>(this ICallistoCollectionContext<TDocument> context,
                                                                                        IEnumerable<TDocument> documents, BulkWriteOptions options = null)
            where TDocument : class, IDocumentRoot, IUpdatableDocument<TDocument>
        {
            IEnumerable<TDocument> updatableDocuments = documents.ToList();
            ValidateOperationParameters(updatableDocuments, context, "bulk-update");
            return await Write(context, updatableDocuments.SelectMany(a => a.PendingUpdates, (root, model) => model), options);
        }

        /// <summary>
        /// Insert one document into the MongoDb collection.
        /// </summary>
        /// <param name="context">Mongodb collection context</param>
        /// <param name="document">The document to be inserted</param>
        /// <param name="options"><see cref="InsertOneOptions"/></param>
        /// <typeparam name="TDocument">Type of the document</typeparam>
        /// <returns>The document</returns>
        public static async ValueTask<TDocument> Insert<TDocument>(this ICallistoCollectionContext<TDocument> context, TDocument document, InsertOneOptions options = null)
            where TDocument : class, IDocumentRoot, IInsertableDocument
        {
            ValidateOperationParameters(document, context, "insert-one");
            await context.Collection.InsertOneAsync(document, options);
            return document;
        }

        /// <summary>
        /// Insert one document into the MongoDb collection.
        /// </summary>
        /// <param name="context">Mongodb collection context</param>
        /// <param name="document">The document to be inserted</param>
        /// <param name="handle"><see cref="IClientSessionHandle"/></param>
        /// <param name="options"><see cref="InsertOneOptions"/></param>
        /// <typeparam name="TDocument">Type of the document</typeparam>
        /// <returns>The document</returns>
        public static async ValueTask<TDocument> Insert<TDocument>(this ICallistoCollectionContext<TDocument> context,
                                                                   TDocument document,
                                                                   IClientSessionHandle handle,
                                                                   InsertOneOptions options = null)
            where TDocument : class, IDocumentRoot, IInsertableDocument
        {
            ValidateOperationParameters(document, context, "insert-one");
            await context.Collection.InsertOneAsync(handle, document, options);
            return document;
        }

        /// <summary>
        /// Delete one document from the MongoDb collection.
        /// </summary>
        /// <param name="context">Mongodb collection context</param>
        /// <param name="document">The document to be delete</param>
        /// <param name="options"><see cref="DeleteOptions"/></param>
        /// <typeparam name="TDocument">Type of the document</typeparam>
        /// <returns><see cref="DeleteResult"/></returns>
        public static async ValueTask<DeleteResult> Delete<TDocument>(this ICallistoCollectionContext<TDocument> context, TDocument document, DeleteOptions options = null)
            where TDocument : class, IDocumentRoot, IDeletableDocument
        {
            ValidateOperationParameters(document, context, "delete-one");
            return await context.Collection.DeleteOneAsync(document.FilterById(), options);
        }

        /// <summary>
        /// Delete one document from the MongoDb collection.
        /// </summary>
        /// <param name="context">Mongodb collection context</param>
        /// <param name="document">The document to be delete</param>
        /// <param name="handle"><see cref="IClientSessionHandle"/></param>
        /// <param name="options"><see cref="DeleteOptions"/></param>
        /// <typeparam name="TDocument">Type of the document</typeparam>
        /// <returns><see cref="DeleteResult"/></returns>
        public static async ValueTask<DeleteResult> Delete<TDocument>(this ICallistoCollectionContext<TDocument> context, TDocument document,
                                                                      IClientSessionHandle handle, DeleteOptions options = null)
            where TDocument : class, IDocumentRoot, IDeletableDocument
        {
            ValidateOperationParameters(document, context, "delete-one");
            return await context.Collection.DeleteOneAsync(handle, document.FilterById(), options);
        }

        /// <summary>
        /// Update one document from the MongoDb collection.
        /// </summary>
        /// <param name="context">Mongodb collection context</param>
        /// <param name="document">The document to be updated</param>
        /// <param name="options"><see cref="UpdateOptions"/></param>
        /// <typeparam name="TDocument">Type of the document</typeparam>
        /// <returns><see cref="BulkWriteResult{TDocument}"/></returns>
        public static async ValueTask<BulkWriteResult<TDocument>> Update<TDocument>(this ICallistoCollectionContext<TDocument> context, TDocument document,
                                                                                    BulkWriteOptions options = null)
            where TDocument : class, IDocumentRoot, IUpdatableDocument<TDocument>
        {
            ValidateOperationParameters(document, context, "update-one");
            return await Write(context, CheckPendingUpdatesListFilter(document), options);
        }

        /// <summary>
        /// Update one document from the MongoDb collection.
        /// </summary>
        /// <param name="context">Mongodb collection context</param>
        /// <param name="document">The document to be updated</param>
        /// <param name="handle"><see cref="IClientSessionHandle"/></param>
        /// <param name="options"><see cref="UpdateOptions"/></param>
        /// <typeparam name="TDocument">Type of the document</typeparam>
        /// <returns><see cref="BulkWriteResult{TDocument}"/></returns>
        public static async ValueTask<BulkWriteResult<TDocument>> Update<TDocument>(this ICallistoCollectionContext<TDocument> context, TDocument document,
                                                                                    IClientSessionHandle handle, BulkWriteOptions options = null)
            where TDocument : class, IDocumentRoot, IUpdatableDocument<TDocument>
        {
            ValidateOperationParameters(document, context, "update-one");

            return await WriteWithHandle(context, CheckPendingUpdatesListFilter(document), handle, options);
        }


        /// <summary>
        /// Replace one document from the MongoDb collection.
        /// </summary>
        /// <param name="context">Mongodb collection context</param>
        /// <param name="document">The document to be replaced</param>
        /// <param name="options"><see cref="ReplaceOptions"/></param>
        /// <typeparam name="TDocument">Type of the document</typeparam>
        /// <returns><see cref="ReplaceOneResult"/></returns>
        public static async ValueTask<ReplaceOneResult> Replace<TDocument>(this ICallistoCollectionContext<TDocument> context, TDocument document,
                                                                           ReplaceOptions options = null)
            where TDocument : class, IDocumentRoot, ICallistoCollectionContext<TDocument>
        {
            ValidateOperationParameters(document, context, "replace-one");
            return await context.Collection.ReplaceOneAsync(document.FilterById(), document, options);
        }

        /// <summary>
        /// Replace one document from the MongoDb collection.
        /// </summary>
        /// <param name="context">Mongodb collection context</param>
        /// <param name="document">The document to be replaced</param>
        /// <param name="handle"><see cref="IClientSessionHandle"/></param>
        /// <param name="options"><see cref="ReplaceOptions"/></param>
        /// <typeparam name="TDocument">Type of the document</typeparam>
        /// <returns><see cref="ReplaceOneResult"/></returns>
        public static async ValueTask<ReplaceOneResult> Replace<TDocument>(this ICallistoCollectionContext<TDocument> context, TDocument document,
                                                                           IClientSessionHandle handle, ReplaceOptions options = null)
            where TDocument : class, IDocumentRoot, ICallistoCollectionContext<TDocument>
        {
            ValidateOperationParameters(document, context, "replace-one");
            return await context.Collection.ReplaceOneAsync(handle, document.FilterById(), document, options);
        }

        private static async ValueTask<BulkWriteResult<T>> Write<T>(ICallistoCollectionContext<T> context, IEnumerable<WriteModel<T>> models, BulkWriteOptions options)
            where T : class, IDocumentRoot
        {
            return await context.Collection.BulkWriteAsync(models, options);
        }

        private static async ValueTask<BulkWriteResult<T>> WriteWithHandle<T>(ICallistoCollectionContext<T> context,
                                                                              IEnumerable<WriteModel<T>> models,
                                                                              IClientSessionHandle handle, BulkWriteOptions options)
            where T : class, IDocumentRoot
        {
            return await context.Collection.BulkWriteAsync(handle, models, options);
        }

        private static void ValidateOperationParameters<T>(IEnumerable<T> documents, ICallistoCollectionContext<T> context, string operation)
            where T : class, IDocumentRoot
        {
            Check.ThrowIfNull(documents,
                              nameof(T),
                              new CallistoException($"Documents collection instance cannot be null. Please provide a collection instance to be able to execute the {operation}"));

            Check.ThrowIfNullElement(documents, nameof(IEnumerable<T>),
                                     new CallistoException($"The documents collection cannot contain null elements. The {operation} will be halted"));
            Check.ThrowIfNull(context,
                              nameof(ICallistoCollectionContext<T>),
                              new CallistoException($"Collection context instance cannot be null. Please provide a valid context instance to be able to execute the {operation}"));
        }

        private static void ValidateOperationParameters<T>(T document, ICallistoCollectionContext<T> context, string operation)
            where T : class, IDocumentRoot
        {
            Check.ThrowIfNull(document,
                              nameof(T),
                              new CallistoException($"Document instance cannot be null. Please provide a document instance to be able to execute the {operation}"));
            Check.ThrowIfNull(context,
                              nameof(ICallistoCollectionContext<T>),
                              new CallistoException($"Collection context instance cannot be null. Please provide a valid context instance to be able to execute the {operation}"));
        }

        private static IEnumerable<UpdateOneModel<TDocument>> CheckPendingUpdatesListFilter<TDocument>(TDocument document)
            where TDocument : class, IDocumentRoot, IUpdatableDocument<TDocument>
        {
            document.PendingUpdates.Any(a => { Check.ThrowIfNull(a.Filter); return false; });
            return document.PendingUpdates;
        }

        private static UpdateOneModel<TDocument> RecreateUpdateOneModel<TDocument>(TDocument document, UpdateOneModel<TDocument> a)
            where TDocument : class, IDocumentRoot, IUpdatableDocument<TDocument>
        {
            return new UpdateOneModel<TDocument>(document.FilterById(), a.Update)
            {
                Collation = a.Collation,
                Hint = a.Hint,
                ArrayFilters = a.ArrayFilters,
                IsUpsert = a.IsUpsert
            };
        }
    }
}
