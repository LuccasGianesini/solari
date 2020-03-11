using System;
using System.Diagnostics.CodeAnalysis;
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
        public CancellationToken CancellationToken { get; }

        public void ValidateOperation()
        {
            if (PipelineDefinition == null)
                throw new NullPipelineDefinitionException("The operation contains a null pipeline definition.");
            if (ResultFunction == null)
                throw new NullResultFunctionException("A null result function will cause the return of a disposed IAsyncCursor." +
                                                      "Please specify a valid result function.");
        }

        public IClientSessionHandle ClientSessionHandle { get; }
        public bool UseSessionHandle { get; }
        public Func<IAsyncCursor<TProjectionModel>, TResult> ResultFunction { get; }
        public PipelineDefinition<T, TProjectionModel> PipelineDefinition { get; }
        public AggregateOptions AggregateOptions { get; }
        public static ICallistoAggregation<T, TProjectionModel, TResult> Null() => new DefaultCallistoAggregation<T, TProjectionModel, TResult>("", null, null);
    }
}