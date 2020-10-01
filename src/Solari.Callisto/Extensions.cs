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
using Solari.Sol;

namespace Solari.Callisto
{
    public static class Extensions
    {
        public static ICallistoOperation<T> UseSessionHandle<T>(this ICallistoOperation<T> operation, IClientSessionHandle sessionHandle)
            where T : class, IDocumentRoot
        {
            Check.ThrowIfNull(operation, nameof(ICallistoOperation<T>), new CallistoException($"Cannot add a {nameof(IClientSessionHandle)} to a null operation instance"));
            Check.ThrowIfNull(operation, nameof(IClientSessionHandle),
                              new CallistoException($"A {nameof(IClientSessionHandle)} must not be null. Start a session then call this method."));
            operation.ClientSessionHandle = sessionHandle;
            return operation;
        }

        public static ICallistoOperation<T> UseCancellationToken<T>(this ICallistoOperation<T> operation, CancellationToken cancellationToken)
            where T : class, IDocumentRoot
        {
            Check.ThrowIfNull(operation, nameof(ICallistoOperation<T>), new CallistoException($"Cannot add a {nameof(CancellationToken)} to a null operation instance"));
            if (cancellationToken == CancellationToken.None)
                return operation;
            operation.CancellationToken = cancellationToken;
            return operation;
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
    }
}
