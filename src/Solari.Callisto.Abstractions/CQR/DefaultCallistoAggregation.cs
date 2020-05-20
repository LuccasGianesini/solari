using System;
using System.Threading;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.Exceptions;

namespace Solari.Callisto.Abstractions.CQR
{
    public class DefaultCallistoAggregation<T, TProjectionModel, TResult> : ICallistoAggregation<T, TProjectionModel, TResult> where T : class, IDocumentRoot
    {
        public DefaultCallistoAggregation(string operationName, Func<IAsyncCursor<TProjectionModel>, TResult> resultFunction,
                                          PipelineDefinition<T, TProjectionModel> pipelineDefinition,
                                          AggregateOptions aggregateOptions = null, IClientSessionHandle clientSessionHandle = null,
                                          CancellationToken? cancellationToken = null)
        {
            OperationName = operationName;
            OperationType = CallistoOperation.Aggregation;
            CancellationToken = cancellationToken ?? CancellationToken.None;
            ClientSessionHandle = clientSessionHandle;
            ResultFunction = resultFunction;
            PipelineDefinition = pipelineDefinition;
            AggregateOptions = aggregateOptions;
            UseSessionHandle = ClientSessionHandle != null;
        }

        public string OperationName { get; }
        public CallistoOperation OperationType { get; }
        public CancellationToken CancellationToken { get; private set; }
        public IClientSessionHandle ClientSessionHandle { get; private set; }
        public bool UseSessionHandle { get; private set; }
        public Func<IAsyncCursor<TProjectionModel>, TResult> ResultFunction { get; }
        public PipelineDefinition<T, TProjectionModel> PipelineDefinition { get; }
        public AggregateOptions AggregateOptions { get; }


        public void ValidateOperation()
        {
            if (PipelineDefinition == null)
                throw new NullPipelineDefinitionException("The operation contains a null pipeline definition.");
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

        public static ICallistoAggregation<T, TProjectionModel, TResult> Null()
        {
            return new DefaultCallistoAggregation<T, TProjectionModel, TResult>("", null, null);
        }
    }
}