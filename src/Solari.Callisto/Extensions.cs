using System.Threading;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.Exceptions;

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
    }
}
