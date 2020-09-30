using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Callisto.Serializers;

namespace Solari.Callisto
{
    public static class Extensions
    {
        public static ICallistoOperation<T> UseSessionHandle<T>(this ICallistoOperation<T> operation, IClientSessionHandle sessionHandle)
            where T : class, IDocumentRoot
        {
            if (operation is null)
                throw new CallistoException($"Cannot add a {nameof(IClientSessionHandle)} to a null operation instance");
            if (sessionHandle is null)
                throw new CallistoException($"A {nameof(IClientSessionHandle)} must not be null. Start a session then call this method.");
            operation.ClientSessionHandle = sessionHandle;
            return operation;
        }

        public static ICallistoOperation<T> UseCancellationToken<T>(this ICallistoOperation<T> operation, CancellationToken cancellationToken)
            where T : class, IDocumentRoot
        {
            if (operation is null)
                throw new CallistoException($"Cannot add a {nameof(CancellationToken)} to a null operation instance");
            if (cancellationToken == CancellationToken.None)
                return operation;
            operation.CancellationToken = cancellationToken;
            return operation;
        }

        public static async Task<T> ExecuteInsert<T>(this T document, ICallistoCollectionContext<T> context)
            where T : class, IDocumentRoot, IInsertableDocument
        {
            ValidateOperationParameters(document, context, "insert-one");
            await context.Collection.InsertOneAsync(document);
            return document;
        }

        public static async Task<DeleteResult> ExecuteDelete<T>(this T document, ICallistoCollectionContext<T> context)
            where T : class, IDocumentRoot, IDeletableDocument
        {
            ValidateOperationParameters(document, context, "delete-one");
            return await context.Collection.DeleteOneAsync(Builders<T>.Filter.Eq(a => a.Id, document.Id));
        }

        public static async Task<BulkWriteResult> ExecuteUpdate<T>(this T document, ICallistoCollectionContext<T> context)
            where T : class, IDocumentRoot, IUpdatableDocument<T>
        {
            ValidateOperationParameters(document, context, "update-one");
            return await context.Collection.BulkWriteAsync(document.PendingUpdates);
        }

        public static async Task<ReplaceOneResult> ExecuteReplace<T>(this T document, ICallistoCollectionContext<T> context)
            where T : class, IDocumentRoot, ICallistoCollectionContext<T>
        {
            ValidateOperationParameters(document, context, "replace-one");
            return await context.Collection.ReplaceOneAsync(Builders<T>.Filter.Eq(a => a.Id, document.Id), document);
        }

        public static BsonClassMap<TClass> MapCallistoDatetimeOffset<TClass, TMember>(this BsonClassMap<TClass> map, Expression<Func<TClass, TMember>> memberLambda)
            where TClass : class, IDocumentRoot
        {
            map.MapMember(memberLambda)
               .SetSerializer(DateTimeOffsetSerializerCustom.Instance);
            return map;
        }

        public static BsonClassMap<TClass> MapCallistoId<TClass>(this BsonClassMap<TClass> map) where TClass : class, IDocumentRoot
        {
            map.MapIdField(a => a.Id)
               .SetIdGenerator(new GuidGenerator())
               .SetIsRequired(true)
               .SetSerializer(new GuidSerializer())
               .SetOrder(0)
               .SetElementName("_id");
            return map;
        }

        private static void ValidateOperationParameters<T>(T document, ICallistoCollectionContext<T> context, string operation)
            where T : class, IDocumentRoot
        {
            if (document is null)
                throw new CallistoException($"Document instance cannot be null. Please provide a document instance to be able to execute the {operation}");
            if (context is null)
                throw new CallistoException($"Collection context instance cannot be null. Please provide a valid context instance to be able to execute the {operation}");
        }
    }
}
