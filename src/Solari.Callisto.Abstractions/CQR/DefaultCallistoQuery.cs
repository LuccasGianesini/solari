using System;
using System.Threading;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.Exceptions;

namespace Solari.Callisto.Abstractions.CQR
{
    public class DefaultCallistoQuery<T, TResult> : ICallistoQuery<T, TResult> where T : class, IDocumentRoot
    {
        public DefaultCallistoQuery(string operationName, FilterDefinition<T> filterDefinition, Func<IAsyncCursor<T>, TResult> resultFunction,
                                    FindOptions<T> findOptions = null, IClientSessionHandle sessionHandle = null,
                                    CancellationToken? cancellationToken = null)
        {
            OperationName = operationName;
            CancellationToken = cancellationToken ?? CancellationToken.None;
            FindOptions = findOptions;
            FilterDefinition = filterDefinition;
            ResultFunction = resultFunction;
            OperationType = CallistoOperation.Query;
            ClientSessionHandle = sessionHandle;
            UseSessionHandle = ClientSessionHandle != null;
        }

        public string OperationName { get; }

        public CallistoOperation OperationType { get; }
        public CancellationToken CancellationToken { get; }

        public void ValidateOperation()
        {
            if (FilterDefinition == null)
                throw new NullFilterDefinitionException(CallistoOperationHelper.NullDefinitionMessage("query", OperationName, "filter"));
            if (ResultFunction == null)
                throw new NullResultFunctionException("A null result function will cause the return of a disposed IAsyncCursor." +
                                                      "Please specify a valid result function.");
        }

        public IClientSessionHandle ClientSessionHandle { get; }
        public bool UseSessionHandle { get; }
        public FindOptions<T> FindOptions { get; }
        public FilterDefinition<T> FilterDefinition { get; }
        public Func<IAsyncCursor<T>, TResult> ResultFunction { get; }

        public static ICallistoQuery<T, TResult> Null() => new DefaultCallistoQuery<T, TResult>("", null, null);
    }
}