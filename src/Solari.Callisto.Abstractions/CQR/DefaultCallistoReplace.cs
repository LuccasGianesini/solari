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
        public CancellationToken CancellationToken { get; }

        public void ValidateOperation()
        {
            if (Replacement == null)
                throw new ArgumentNullException(nameof(Replacement), "The value of the replace command is null. The operation will not be carried out");
            if (FilterDefinition == null)
                throw new NullFilterDefinitionException(CallistoOperationHelper.NullDefinitionMessage("replace-one", OperationName, "filter"));
        }

        public IClientSessionHandle ClientSessionHandle { get; }
        public bool UseSessionHandle { get; }
        public T Replacement { get; }
        public FilterDefinition<T> FilterDefinition { get; }
        public ReplaceOptions ReplaceOptions { get; }

        public static ICallistoReplace<T> Null() => new DefaultCallistoReplace<T>("", null, null);
    }
}