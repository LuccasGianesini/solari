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
        public CancellationToken CancellationToken { get; }

        public void ValidateOperation()
        {
            if (FilterDefinition == null)
                throw new NullFilterDefinitionException(CallistoOperationHelper.NullDefinitionMessage("delete", OperationName, "filter"));
        }

        public IClientSessionHandle ClientSessionHandle { get; }
        public bool UseSessionHandle { get; }

        public FilterDefinition<T> FilterDefinition { get; }
        public DeleteOptions DeleteOptions { get; }

        public static ICallistoDelete<T> Null() => new DefaultCallistoDelete<T>("", null);
    }
}