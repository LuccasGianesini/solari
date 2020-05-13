using System;
using System.Collections.Generic;
using System.Threading;
using MongoDB.Bson;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.CQR;

namespace Solari.Callisto
{
    public class CallistoOperationFactory : ICallistoOperationFactory
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
        public ICallistoAggregation<T, TProjectionModel, TResult> CreateAggregation<T, TProjectionModel, TResult>(PipelineDefinition<T, TProjectionModel>
                                                                                                                      pipelineDefinition,
                                                                                                                  Func<IAsyncCursor<TProjectionModel>, TResult>
                                                                                                                      resultFunction,
                                                                                                                  AggregateOptions options = null,
                                                                                                                  IClientSessionHandle sessionHandle = null,
                                                                                                                  string operationName = "",
                                                                                                                  CancellationToken? cancellationToken = null)
            where T : class, IDocumentRoot
        {
            return pipelineDefinition == null
                       ? DefaultCallistoAggregation<T, TProjectionModel, TResult>.Null()
                       : resultFunction == null
                           ? DefaultCallistoAggregation<T, TProjectionModel, TResult>.Null()
                           : new DefaultCallistoAggregation<T, TProjectionModel, TResult>(operationName, resultFunction, pipelineDefinition, options,
                                                                                          sessionHandle, cancellationToken);
        }

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
        public ICallistoQuery<T, TResult> CreateByIdQuery<T, TResult>(ObjectId id,
                                                                      Func<IAsyncCursor<T>, TResult> resultFunction,
                                                                      FindOptions<T> findOptions = null, IClientSessionHandle sessionHandle = null,
                                                                      string operationName = "",
                                                                      CancellationToken? cancellationToken = null) where T : class, IDocumentRoot
        {
            return id.Equals(CallistoConstants.ObjectIdDefaultValue)
                       ? DefaultCallistoQuery<T, TResult>.Null()
                       : CreateQuery(Builders<T>.Filter.Eq(a => a.Id, id), resultFunction, findOptions, sessionHandle, operationName, cancellationToken);
        }

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
        public ICallistoQuery<T, TResult> CreateQuery<T, TResult>(FilterDefinition<T> filterDefinition,
                                                                  Func<IAsyncCursor<T>, TResult> resultFunction,
                                                                  FindOptions<T> findOptions = null, IClientSessionHandle sessionHandle = null,
                                                                  string operationName = "",
                                                                  CancellationToken? cancellationToken = null) where T : class, IDocumentRoot
        {
            return filterDefinition == null
                       ? DefaultCallistoQuery<T, TResult>.Null()
                       : resultFunction == null
                           ? DefaultCallistoQuery<T, TResult>.Null()
                           : new DefaultCallistoQuery<T, TResult>(operationName, filterDefinition, resultFunction, findOptions, sessionHandle,
                                                                  cancellationToken);
        }

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
        public ICallistoReplace<T> CreateReplace<T>(T replacement, FilterDefinition<T> filterDefinition,
                                                    ReplaceOptions replaceOptions = null, IClientSessionHandle sessionHandle = null,
                                                    string operationName = "",
                                                    CancellationToken? cancellationToken = null) where T : class, IDocumentRoot
        {
            return replacement == null
                       ? DefaultCallistoReplace<T>.Null()
                       : filterDefinition == null
                           ? DefaultCallistoReplace<T>.Null()
                           : new DefaultCallistoReplace<T>(operationName, replacement, filterDefinition, replaceOptions, sessionHandle, cancellationToken);
        }

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
        public ICallistoReplace<T> CreateReplaceById<T>(T replacement, ObjectId id,
                                                        ReplaceOptions replaceOptions = null, IClientSessionHandle sessionHandle = null,
                                                        string operationName = "",
                                                        CancellationToken? cancellationToken = null) where T : class, IDocumentRoot
        {
            return id.Equals(CallistoConstants.ObjectIdDefaultValue)
                       ? DefaultCallistoReplace<T>.Null()
                       : CreateReplace(replacement, Builders<T>.Filter.Eq(a => a.Id, id), replaceOptions,
                                       sessionHandle, operationName, cancellationToken);
        }

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
        public ICallistoDelete<T> CreateDelete<T>(FilterDefinition<T> filterDefinition, DeleteOptions deleteOptions = null,
                                                  IClientSessionHandle sessionHandle = null, string operationName = "",
                                                  CancellationToken? cancellationToken = null)
            where T : class, IDocumentRoot
        {
            return filterDefinition == null
                       ? DefaultCallistoDelete<T>.Null()
                       : new DefaultCallistoDelete<T>(operationName, filterDefinition, deleteOptions, sessionHandle, cancellationToken);
        }

        /// <summary>
        ///     Create an delete command.
        /// </summary>
        /// <param name="operationName">The name of the operation</param>
        /// <param name="id"></param>
        /// <param name="deleteOptions">Options</param>
        /// <param name="sessionHandle">Session handle</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <typeparam name="T">Type of the entity</typeparam>
        /// <returns>
        ///     <see cref="ICallistoDelete{T}" />
        /// </returns>
        public ICallistoDelete<T> CreateDeleteById<T>(ObjectId id, DeleteOptions deleteOptions = null,
                                                      IClientSessionHandle sessionHandle = null, string operationName = "",
                                                      CancellationToken? cancellationToken = null)
            where T : class, IDocumentRoot
        {
            return id.Equals(CallistoConstants.ObjectIdDefaultValue)
                       ? DefaultCallistoDelete<T>.Null()
                       : CreateDelete(Builders<T>.Filter.Eq(a => a.Id, id), deleteOptions, sessionHandle, operationName, cancellationToken);
        }

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
        public ICallistoInsert<T> CreateInsert<T>(T value,
                                                  InsertOneOptions insertOneOptions = null, IClientSessionHandle sessionHandle = null,
                                                  string operationName = "",
                                                  CancellationToken? cancellationToken = null) where T : class, IDocumentRoot
        {
            return new DefaultCallistoInsert<T>(operationName, value, sessionHandle, insertOneOptions, cancellationToken);
        }

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
        public ICallistoInsertMany<T> CreateInsertMany<T>(IEnumerable<T> values,
                                                          InsertManyOptions insertManyOptions = null, IClientSessionHandle sessionHandle = null,
                                                          string operationName = "",
                                                          CancellationToken? cancellationToken = null) where T : class, IDocumentRoot
        {
            return new DefaultCallistoInsertMany<T>(operationName, values, insertManyOptions,
                                                    sessionHandle, cancellationToken);
        }


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
        public ICallistoUpdate<T> CreateUpdate<T>(UpdateDefinition<T> updateDefinition,
                                                  FilterDefinition<T> filterDefinition, UpdateOptions updateOptions = null,
                                                  IClientSessionHandle sessionHandle = null, string operationName = "",
                                                  CancellationToken? cancellationToken = null)
            where T : class, IDocumentRoot
        {
            return updateDefinition == null || filterDefinition == null
                       ? DefaultCallistoUpdate<T>.Null()
                       : new DefaultCallistoUpdate<T>(operationName, updateDefinition, filterDefinition, updateOptions,
                                                      sessionHandle, cancellationToken);
        }

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
        public ICallistoUpdate<T> CreateUpdateById<T>(ObjectId id,
                                                      UpdateDefinition<T> updateDefinition, UpdateOptions updateOptions = null,
                                                      IClientSessionHandle sessionHandle = null, string operationName = "",
                                                      CancellationToken? cancellationToken = null)
            where T : class, IDocumentRoot
        {
            return id.Equals(CallistoConstants.ObjectIdDefaultValue)
                       ? DefaultCallistoUpdate<T>.Null()
                       : CreateUpdate(updateDefinition, Builders<T>.Filter.Eq(a => a.Id, id), updateOptions,
                                      sessionHandle, operationName, cancellationToken);
        }
    }
}