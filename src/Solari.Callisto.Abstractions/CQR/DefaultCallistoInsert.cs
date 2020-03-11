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
        public CancellationToken CancellationToken { get; }

        public void 
            ValidateOperation()
        {
            if (Value == null)
                throw new NullOrEmptyValueException($"The value property of the {OperationName} is null.");
        }

        public T Value { get; }
        public IClientSessionHandle ClientSessionHandle { get; }
        public InsertOneOptions InsertOneOptions { get; }
        public bool UseSessionHandle { get; }
    }
}