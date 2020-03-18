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
        public CancellationToken CancellationToken { get; private set; }
        public UpdateDefinition<T> UpdateDefinition { get; }
        public FilterDefinition<T> FilterDefinition { get; }
        public UpdateOptions UpdateOptions { get; }
        public IClientSessionHandle ClientSessionHandle { get; private set; }
        public bool UseSessionHandle { get; private set; }

        public void ValidateOperation()
        {
            if (UpdateDefinition == null)
                throw new NullUpdateDefinitionException(CallistoOperationHelper.NullDefinitionMessage("update", OperationName, "update"));
            if (FilterDefinition == null)
                throw new NullFilterDefinitionException(CallistoOperationHelper.NullDefinitionMessage("update", OperationName, "filter"));
        }

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

        public static DefaultCallistoUpdate<T> Null() { return new DefaultCallistoUpdate<T>("", null, null); }
    }
}