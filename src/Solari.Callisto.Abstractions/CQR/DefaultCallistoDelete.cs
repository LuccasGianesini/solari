using System.Threading;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.Exceptions;

namespace Solari.Callisto.Abstractions.CQR
{
    public class DefaultCallistoDelete<T> : ICallistoDelete<T> where T : class, IDocumentRoot
    {
        public DefaultCallistoDelete(string operationName, FilterDefinition<T> filterDefinition, DeleteOptions deleteOptions = null,
                                     IClientSessionHandle sessionHandle = null, CancellationToken? cancellationToken = null)
        {
            OperationName = operationName;
            OperationType = CallistoOperation.Delete;
            CancellationToken = cancellationToken ?? CancellationToken.None;
            FilterDefinition = filterDefinition;
            DeleteOptions = deleteOptions;
            ClientSessionHandle = sessionHandle;
            UseSessionHandle = ClientSessionHandle != null;
        }

        public string OperationName { get; }
        public CallistoOperation OperationType { get; }
        public CancellationToken CancellationToken { get; private set; }

        public void ValidateOperation()
        {
            if (FilterDefinition == null)
                throw new NullFilterDefinitionException(CallistoOperationHelper.NullDefinitionMessage("delete", OperationName, "filter"));
        }

        public IClientSessionHandle ClientSessionHandle { get; private set; }
        public bool UseSessionHandle { get; private set; }

        public FilterDefinition<T> FilterDefinition { get; }
        public DeleteOptions DeleteOptions { get; }

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

        public static ICallistoDelete<T> Null() { return new DefaultCallistoDelete<T>("", null); }
    }
}