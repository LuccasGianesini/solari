using System.Threading;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.Exceptions;

namespace Solari.Callisto.Abstractions.CQR
{
    public sealed class DefaultCallistoUpdate<T> : ICallistoUpdate<T> where T : class, IDocumentRoot
    {
        public DefaultCallistoUpdate(string operationName, UpdateDefinition<T> updateDefinition,
                                     FilterDefinition<T> filterDefinition, UpdateOptions updateOptions = null,
                                     IClientSessionHandle sessionHandle = null, CancellationToken? cancellationToken = null)
        {
            OperationName = operationName;
            OperationType = CallistoOperation.Update;
            CancellationToken = cancellationToken ?? CancellationToken.None;
            UpdateDefinition = updateDefinition;
            FilterDefinition = filterDefinition;
            UpdateOptions = updateOptions;
            ClientSessionHandle = sessionHandle;
            UseSessionHandle = ClientSessionHandle != null;
        }

        public string OperationName { get; }
        public CallistoOperation OperationType { get; }
        public CancellationToken CancellationToken { get; }
        public UpdateDefinition<T> UpdateDefinition { get; }
        public FilterDefinition<T> FilterDefinition { get; }
        public UpdateOptions UpdateOptions { get; }


        public void ValidateOperation()
        {
            if (UpdateDefinition == null)
                throw new NullUpdateDefinitionException(CallistoOperationHelper.NullDefinitionMessage("update", OperationName, "update"));
            if (FilterDefinition == null)
                throw new NullFilterDefinitionException(CallistoOperationHelper.NullDefinitionMessage("update", OperationName, "filter"));
        }

        public IClientSessionHandle ClientSessionHandle { get; }
        public bool UseSessionHandle { get; }

        public static DefaultCallistoUpdate<T> Null() => new DefaultCallistoUpdate<T>("", null, null, null, null);
    }
}