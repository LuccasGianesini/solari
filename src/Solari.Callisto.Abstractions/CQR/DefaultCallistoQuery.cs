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
        public CancellationToken CancellationToken { get; private set; }
        public IClientSessionHandle ClientSessionHandle { get; private set; }
        public bool UseSessionHandle { get; private set; }
        public FindOptions<T> FindOptions { get; }
        public FilterDefinition<T> FilterDefinition { get; }
        public Func<IAsyncCursor<T>, TResult> ResultFunction { get; }


        public void ValidateOperation()
        {
            if (FilterDefinition == null)
                throw new NullFilterDefinitionException(CallistoOperationHelper.NullDefinitionMessage("query", OperationName, "filter"));
            if (ResultFunction == null)
                throw new NullResultFunctionException("A null result function will cause the return of a disposed IAsyncCursor." +
                                                      "Please specify a valid result function.");
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

        public static ICallistoQuery<T, TResult> Null() { return new DefaultCallistoQuery<T, TResult>("", null, null); }
    }
}