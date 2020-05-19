using System.Threading;
using MongoDB.Driver;

namespace Solari.Callisto.Abstractions
{
    public interface ICallistoOperation<T> where T : class, IDocumentRoot
    {
        string OperationName { get; }
        CallistoOperation OperationType { get; }
        CancellationToken CancellationToken { get; }
        IClientSessionHandle ClientSessionHandle { get; }
        bool UseSessionHandle { get; }
        void ValidateOperation();
        ICallistoOperation<T> AddSessionHandle(IClientSessionHandle sessionHandle);
        ICallistoOperation<T> AddCancellationToken(CancellationToken cancellationToken);
    }
}