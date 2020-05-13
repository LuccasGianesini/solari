using System;
using System.Collections.Generic;
using System.Threading;
using MongoDB.Bson;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.CQR;

namespace Solari.Callisto.Abstractions
{
    public interface ICallistoOperationFactory
    {
        /// <summary>
        ///     Create an aggregate.
        /// </summary>
        /// <param name="operationName">The name of the operation</param>
        /// <param name="pipelineDefinition">The <see cref="PipelineDefinition{TInput,TOutput}" /></param>
        /// <param name="resultFunction">The result function</param>
        /// <param name="options">Options</param>
        /// <param name="sessionHandle">Session Handle</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <typeparam name="T">Type of the entity</typeparam>
        /// <typeparam name="TProjectionModel">Type of the projection model</typeparam>
        /// <typeparam name="TResult">The final result of the query</typeparam>
        /// <returns>
        ///     <see cref="ICallistoAggregation{T,TProjectionModel,TResult}" />
        /// </returns>
        ICallistoAggregation<T, TProjectionModel, TResult> CreateAggregation<T, TProjectionModel, TResult>(PipelineDefinition<T, TProjectionModel>
                                                                                                               pipelineDefinition,
                                                                                                           Func<IAsyncCursor<TProjectionModel>, TResult>
                                                                                                               resultFunction,
                                                                                                           AggregateOptions options = null,
                                                                                                           IClientSessionHandle sessionHandle = null,
                                                                                                           string operationName = "",
                                                                                                           CancellationToken? cancellationToken = null)
            where T : class, IDocumentRoot;

        /// <summary>
        ///     Create query by id.
        /// </summary>
        /// <param name="operationName">Operation Name</param>
        /// <param name="id">The filter for the query</param>
        /// <param name="resultFunction">The result function</param>
        /// <param name="findOptions">Options</param>
        /// <param name="sessionHandle">Session Handle</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <typeparam name="T">Type of the entity</typeparam>
        /// <typeparam name="TResult">Type of the query result</typeparam>
        /// <returns>
        ///     <see cref="ICallistoQuery{T,TResult}" />
        /// </returns>
        ICallistoQuery<T, TResult> CreateByIdQuery<T, TResult>(ObjectId id,
                                                               Func<IAsyncCursor<T>, TResult> resultFunction,
                                                               FindOptions<T> findOptions = null, IClientSessionHandle sessionHandle = null,
                                                               string operationName = "", CancellationToken? cancellationToken = null)
            where T : class, IDocumentRoot;

        /// <summary>
        ///     Create a query.
        /// </summary>
        /// <param name="operationName">Operation Name</param>
        /// <param name="filterDefinition">The filter for the query</param>
        /// <param name="resultFunction">The result function</param>
        /// <param name="findOptions">Options</param>
        /// <param name="sessionHandle">Session Handle</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <typeparam name="T">Type of the entity</typeparam>
        /// <typeparam name="TResult">Type of the query result</typeparam>
        /// <returns>
        ///     <see cref="ICallistoQuery{T,TResult}" />
        /// </returns>
        ICallistoQuery<T, TResult> CreateQuery<T, TResult>(FilterDefinition<T> filterDefinition,
                                                           Func<IAsyncCursor<T>, TResult> resultFunction,
                                                           FindOptions<T> findOptions = null, IClientSessionHandle sessionHandle = null,
                                                           string operationName = "", CancellationToken? cancellationToken = null)
            where T : class, IDocumentRoot;

        /// <summary>
        ///     Create a replace command.
        /// </summary>
        /// <param name="operationName">The name of the operation</param>
        /// <param name="replacement">The replacement entity</param>
        /// <param name="filterDefinition">The filter</param>
        /// <param name="replaceOptions">Options</param>
        /// <param name="sessionHandle">Session Handle</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <typeparam name="T">The type of the entity</typeparam>
        /// <returns>
        ///     <see cref="ICallistoReplace{T}" />
        /// </returns>
        ICallistoReplace<T> CreateReplace<T>(T replacement, FilterDefinition<T> filterDefinition,
                                             ReplaceOptions replaceOptions = null, IClientSessionHandle sessionHandle = null,
                                             string operationName = "", CancellationToken? cancellationToken = null) where T : class, IDocumentRoot;

        /// <summary>
        ///     Create a replace by id command.
        /// </summary>
        /// <param name="operationName">The name of the operation</param>
        /// <param name="replacement">The replacement entity</param>
        /// <param name="id">The id of the document</param>
        /// <param name="replaceOptions">Options</param>
        /// <param name="sessionHandle">Session Handle</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <typeparam name="T">The type of the entity</typeparam>
        /// <returns>
        ///     <see cref="ICallistoReplace{T}" />
        /// </returns>
        ICallistoReplace<T> CreateReplaceById<T>(T replacement, ObjectId id,
                                                 ReplaceOptions replaceOptions = null, IClientSessionHandle sessionHandle = null,
                                                 string operationName = "", CancellationToken? cancellationToken = null) where T : class, IDocumentRoot;

        /// <summary>
        ///     Create an delete command.
        /// </summary>
        /// <param name="operationName">The name of the operation</param>
        /// <param name="filterDefinition">The filter for the query</param>
        /// <param name="deleteOptions">Options</param>
        /// <param name="sessionHandle">Session handle</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <typeparam name="T">Type of the entity</typeparam>
        /// <returns>
        ///     <see cref="ICallistoDelete{T}" />
        /// </returns>
        ICallistoDelete<T> CreateDelete<T>(FilterDefinition<T> filterDefinition, DeleteOptions deleteOptions = null,
                                           IClientSessionHandle sessionHandle = null, string operationName = "", CancellationToken? cancellationToken = null)
            where T : class, IDocumentRoot;

        /// <summary>
        ///     Create an delete command.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deleteOptions">Options</param>
        /// <param name="sessionHandle">Session handle</param>
        /// <param name="operationName">The name of the operation</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <typeparam name="T">Type of the entity</typeparam>
        /// <returns>
        ///     <see cref="ICallistoDelete{T}" />
        /// </returns>
        ICallistoDelete<T> CreateDeleteById<T>(ObjectId id, DeleteOptions deleteOptions = null,
                                               IClientSessionHandle sessionHandle = null, string operationName = "",
                                               CancellationToken? cancellationToken = null)
            where T : class, IDocumentRoot;

        /// <summary>
        ///     Create an InsertOne command.
        /// </summary>
        /// <param name="operationName">The name of the insert operation</param>
        /// <param name="value">Value</param>
        /// <param name="insertOneOptions">Options</param>
        /// <param name="sessionHandle">Session Handle</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <typeparam name="T">Type of the entity</typeparam>
        /// <returns>
        ///     <see cref="ICallistoInsert{T}" />
        /// </returns>
        ICallistoInsert<T> CreateInsert<T>(T value,
                                           InsertOneOptions insertOneOptions = null, IClientSessionHandle sessionHandle = null,
                                           string operationName = "", CancellationToken? cancellationToken = null) where T : class, IDocumentRoot;

        /// <summary>
        ///     Create an InsertMany command.
        /// </summary>
        /// <param name="operationName">The name of the insert many operation</param>
        /// <param name="values">The values array</param>
        /// <param name="insertManyOptions">Options</param>
        /// <param name="sessionHandle">Session Handler</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <returns>
        ///     <see cref="ICallistoInsertMany{T}" />
        /// </returns>
        ICallistoInsertMany<T> CreateInsertMany<T>(IEnumerable<T> values,
                                                   InsertManyOptions insertManyOptions = null, IClientSessionHandle sessionHandle = null,
                                                   string operationName = "", CancellationToken? cancellationToken = null) where T : class, IDocumentRoot;

        /// <summary>
        ///     Create an update command.
        /// </summary>
        /// <param name="operationName">The name of the operation</param>
        /// <param name="filterDefinition">Filter</param>
        /// <param name="updateDefinition">Update</param>
        /// <param name="updateOptions">Options</param>
        /// <param name="sessionHandle"></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>
        ///     <see cref="DefaultCallistoUpdate{T}" />
        /// </returns>
        ICallistoUpdate<T> CreateUpdate<T>(UpdateDefinition<T> updateDefinition,
                                           FilterDefinition<T> filterDefinition, UpdateOptions updateOptions = null,
                                           IClientSessionHandle sessionHandle = null, string operationName = "", CancellationToken? cancellationToken = null)
            where T : class, IDocumentRoot;

        /// <summary>
        ///     Create an update command using the id of the document as filter.
        /// </summary>
        /// <param name="operationName">The name of the operation</param>
        /// <param name="id">Id of the document that will be updated</param>
        /// <param name="updateDefinition">Update</param>
        /// <param name="updateOptions">Options</param>
        /// <param name="sessionHandle"></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>
        ///     <see cref="DefaultCallistoUpdate{T}" />
        /// </returns>
        ICallistoUpdate<T> CreateUpdateById<T>(ObjectId id,
                                               UpdateDefinition<T> updateDefinition, UpdateOptions updateOptions = null,
                                               IClientSessionHandle sessionHandle = null, string operationName = "",
                                               CancellationToken? cancellationToken = null)
            where T : class, IDocumentRoot;
    }
}