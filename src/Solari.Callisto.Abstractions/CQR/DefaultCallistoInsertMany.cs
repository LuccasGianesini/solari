using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.Exceptions;

namespace Solari.Callisto.Abstractions.CQR
{
    public class DefaultCallistoInsertMany<T> : ICallistoInsertMany<T> where T : class, IDocumentRoot
    {
        public DefaultCallistoInsertMany(string operationName, IEnumerable<T> values,
                                         InsertManyOptions insertManyOptions = null, IClientSessionHandle clientSessionHandle = null,
                                         CancellationToken? cancellationToken = null)
        {
            OperationName = operationName;
            OperationType = CallistoOperation.Insert;
            CancellationToken = cancellationToken ?? CancellationToken.None;
            Values = values;
            InsertManyOptions = insertManyOptions;
            ClientSessionHandle = clientSessionHandle;
            UseSessionHandle = ClientSessionHandle != null;
        }

        public string OperationName { get; }
        public CallistoOperation OperationType { get; }
        public CancellationToken CancellationToken { get; }

        public void ValidateOperation()
        {
            if (Values == null || !Values.Any())
                throw new NullOrEmptyValueException($"The values array of the {OperationName} is null or it does not contains any items.");
        }

        public IEnumerable<T> Values { get; }
        public InsertManyOptions InsertManyOptions { get; }
        public IClientSessionHandle ClientSessionHandle { get; }
        public bool UseSessionHandle { get; }

        public static ICallistoInsertMany<T> Null() => new DefaultCallistoInsertMany<T>("", null, null, null);
    }
}