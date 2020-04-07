using System.Threading;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.Exceptions;

namespace Solari.Callisto.Abstractions.CQR
{
    public class DefaultCallistoInsert<T> : ICallistoInsert<T> where T : class, IDocumentRoot
    {
        public DefaultCallistoInsert(string operationName, T value, IClientSessionHandle clientSessionHandle = null,
                                     InsertOneOptions insertOneOptions = null, CancellationToken? cancellationToken = null)
        {
            OperationName = operationName;
            CancellationToken = cancellationToken ?? CancellationToken.None;
            Value = value;
            ClientSessionHandle = clientSessionHandle;
            InsertOneOptions = insertOneOptions;
            OperationType = CallistoOperation.Insert;
            UseSessionHandle = ClientSessionHandle != null;
        }

        public string OperationName { get; }
        public CallistoOperation OperationType { get; }
        public CancellationToken CancellationToken { get; private set; }

        public void ValidateOperation()
        {
            if (Value == null)
                throw new NullOrEmptyValueException($"The value property of the {OperationName} is null.");
        }

        public T Value { get; }
        public IClientSessionHandle ClientSessionHandle { get; private set; }
        public InsertOneOptions InsertOneOptions { get; }
        public bool UseSessionHandle { get; private set; }

        public ICallistoOperation<T> AddSessionHandle(IClientSessionHandle sessionHandle)
        {
            if (sessionHandle == null)
                return this;
            ClientSessionHandle = sessionHandle;
            UseSessionHandle = true;
            return this;
        }

        public ICallistoOperation<T> AddCancellationToken(CancellationToken cancellationToken)
        {
            if (cancellationToken == CancellationToken.None)
                return this;
            CancellationToken = cancellationToken;
            return this;
        }
    }
}