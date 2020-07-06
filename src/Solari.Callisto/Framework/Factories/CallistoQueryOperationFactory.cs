using System;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.CQR;
using Solari.Callisto.Abstractions.Exceptions;

namespace Solari.Callisto.Framework.Factories
{
    public class CallistoQueryOperationFactory : ICallistoQueryOperationFactory
    {
        public ICallistoQuery<T, TResult> CreateQuery<T, TResult>(FilterDefinition<T> filterDefinition, Func<IAsyncCursor<T>, TResult> resultFunction)
            where T : class, IDocumentRoot
        {
            return CreateQuery(filterDefinition, resultFunction, null, string.Empty);
        }

        public ICallistoQuery<T, TResult> CreateQuery<T, TResult>(FilterDefinition<T> filterDefinition, Func<IAsyncCursor<T>, TResult> resultFunction,
                                                                  string operationName)
            where T : class, IDocumentRoot
        {
            return CreateQuery(filterDefinition, resultFunction, null, operationName);
        }

        public ICallistoQuery<T, TResult> CreateQuery<T, TResult>(FilterDefinition<T> filterDefinition, Func<IAsyncCursor<T>, TResult> resultFunction,
                                                                  FindOptions<T> findOptions)
            where T : class, IDocumentRoot
        {
            return CreateQuery(filterDefinition, resultFunction, findOptions, string.Empty);
        }

        public ICallistoQuery<T, TResult> CreateQuery<T, TResult>(FilterDefinition<T> filterDefinition, Func<IAsyncCursor<T>, TResult> resultFunction,
                                                                  FindOptions<T> findOptions, string operationName)
            where T : class, IDocumentRoot
        {
            if (filterDefinition is null)
                throw new CallistoException($"An {nameof(ICallistoQuery<T, TResult>)} requires an {nameof(FilterDefinition<T>)}.");
            if (resultFunction is null)
                throw new CallistoException($"An {nameof(ICallistoQuery<T, TResult>)} " +
                                            $"requires an {nameof(Func<IAsyncCursor<T>, TResult>)}.");
            return new DefaultCallistoQuery<T, TResult>(operationName, filterDefinition, resultFunction, findOptions);
        }

        public ICallistoAggregation<T, TProjectionModel, TResult> CreateAggregation<T, TProjectionModel, TResult>(
            PipelineDefinition<T, TProjectionModel> pipelineDefinition, Func<IAsyncCursor<TProjectionModel>, TResult> resultFunction)
            where T : class, IDocumentRoot
            where TProjectionModel : class
        {
            return CreateAggregation(pipelineDefinition, resultFunction, null, string.Empty);
        }

        public ICallistoAggregation<T, TProjectionModel, TResult> CreateAggregation<T, TProjectionModel, TResult>(
            PipelineDefinition<T, TProjectionModel> pipelineDefinition, Func<IAsyncCursor<TProjectionModel>, TResult> resultFunction, string operationName)
            where T : class, IDocumentRoot
            where TProjectionModel : class
        {
            return CreateAggregation(pipelineDefinition, resultFunction, null, operationName);
        }

        public ICallistoAggregation<T, TProjectionModel, TResult> CreateAggregation<T, TProjectionModel, TResult>(
            PipelineDefinition<T, TProjectionModel> pipelineDefinition, Func<IAsyncCursor<TProjectionModel>, TResult> resultFunction,
            AggregateOptions options)
            where T : class, IDocumentRoot
            where TProjectionModel : class
        {
            return CreateAggregation(pipelineDefinition, resultFunction, options, string.Empty);
        }

        public ICallistoAggregation<T, TProjectionModel, TResult> CreateAggregation<T, TProjectionModel, TResult>(
            PipelineDefinition<T, TProjectionModel> pipelineDefinition, Func<IAsyncCursor<TProjectionModel>, TResult> resultFunction,
            AggregateOptions options, string operationName)
            where T : class, IDocumentRoot
            where TProjectionModel : class
        {
            if (pipelineDefinition is null)
                throw new CallistoException($"An {nameof(ICallistoAggregation<T, TProjectionModel, TResult>)} " +
                                            $"requires instance of {nameof(PipelineDefinition<T, TProjectionModel>)}");
            if (resultFunction is null)
                throw new CallistoException($"An {nameof(ICallistoAggregation<T, TProjectionModel, TResult>)} " +
                                            $"requires an {nameof(Func<IAsyncCursor<TProjectionModel>, TResult>)}.");
            return new DefaultCallistoAggregation<T, TProjectionModel, TResult>(operationName, resultFunction, pipelineDefinition, options);
        }
    }
}