using System;
using System.Threading;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.Exceptions;

namespace Solari.Callisto.Abstractions.CQR
{
    public class DefaultCallistoReplace<T> : ICallistoReplace<T> where T : class, IDocumentRoot
    {
        public DefaultCallistoReplace(string operationName, T replacement, FilterDefinition<T> filterDefinition,
                                      ReplaceOptions replaceOptions = null, IClientSessionHandle clientSessionHandle = null,
                                      CancellationToken? cancellationToken = null)
        {
            OperationName = operationName;
            CancellationToken = cancellationToken ?? CancellationToken.None;
            ClientSessionHandle = clientSessionHandle;
            Replacement = replacement;
            FilterDefinition = filterDefinition;
            ReplaceOptions = replaceOptions;
            OperationType = CallistoOperation.Replace;
            UseSessionHandle = ClientSessionHandle != null;
        }

        public string OperationName { get; }
        public CallistoOperation OperationType { get; }
        public CancellationToken CancellationToken { get; private set; }
        public IClientSessionHandle ClientSessionHandle { get; private set; }
        public bool UseSessionHandle { get; private set; }
        public T Replacement { get; }
        public FilterDefinition<T> FilterDefinition { get; }
        public ReplaceOptions ReplaceOptions { get; }


        public void ValidateOperation()
        {
            if (Replacement == null)
                throw new ArgumentNullException(nameof(Replacement), "The value of the replace command is null. The operation will not be carried out");
            if (FilterDefinition == null)
                throw new NullFilterDefinitionException(CallistoOperationHelper.NullDefinitionMessage("replace-one", OperationName, "filter"));
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

        public static ICallistoReplace<T> Null() { return new DefaultCallistoReplace<T>("", null, null); }
    }
}