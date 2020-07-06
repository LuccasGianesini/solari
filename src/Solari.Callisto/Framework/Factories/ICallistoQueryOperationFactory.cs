using System;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.CQR;

namespace Solari.Callisto.Framework.Factories
{
    public interface ICallistoQueryOperationFactory
    {
        ICallistoQuery<T, TResult> CreateQuery<T, TResult>(FilterDefinition<T> filterDefinition, Func<IAsyncCursor<T>, TResult> resultFunction)
            where T : class, IDocumentRoot;

        ICallistoQuery<T, TResult> CreateQuery<T, TResult>(FilterDefinition<T> filterDefinition, Func<IAsyncCursor<T>, TResult> resultFunction,
                                                           string operationName)
            where T : class, IDocumentRoot;

        ICallistoQuery<T, TResult> CreateQuery<T, TResult>(FilterDefinition<T> filterDefinition, Func<IAsyncCursor<T>, TResult> resultFunction,
                                                           FindOptions<T> findOptions)
            where T : class, IDocumentRoot;

        ICallistoQuery<T, TResult> CreateQuery<T, TResult>(FilterDefinition<T> filterDefinition, Func<IAsyncCursor<T>, TResult> resultFunction,
                                                           FindOptions<T> findOptions, string operationName)
            where T : class, IDocumentRoot;

        ICallistoAggregation<T, TProjectionModel, TResult> CreateAggregation<T, TProjectionModel, TResult>(
            PipelineDefinition<T, TProjectionModel> pipelineDefinition, Func<IAsyncCursor<TProjectionModel>, TResult> resultFunction)
            where T : class, IDocumentRoot
            where TProjectionModel : class;

        ICallistoAggregation<T, TProjectionModel, TResult> CreateAggregation<T, TProjectionModel, TResult>(
            PipelineDefinition<T, TProjectionModel> pipelineDefinition, Func<IAsyncCursor<TProjectionModel>, TResult> resultFunction, string operationName)
            where T : class, IDocumentRoot
            where TProjectionModel : class;

        ICallistoAggregation<T, TProjectionModel, TResult> CreateAggregation<T, TProjectionModel, TResult>(
            PipelineDefinition<T, TProjectionModel> pipelineDefinition, Func<IAsyncCursor<TProjectionModel>, TResult> resultFunction,
            AggregateOptions options)
            where T : class, IDocumentRoot
            where TProjectionModel : class;

        ICallistoAggregation<T, TProjectionModel, TResult> CreateAggregation<T, TProjectionModel, TResult>(
            PipelineDefinition<T, TProjectionModel> pipelineDefinition, Func<IAsyncCursor<TProjectionModel>, TResult> resultFunction,
            AggregateOptions options, string operationName)
            where T : class, IDocumentRoot
            where TProjectionModel : class;
    }
}