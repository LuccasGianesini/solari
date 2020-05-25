using System;
using MongoDB.Bson;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.CQR;

namespace Solari.Callisto.Framework
{
    public interface ICallistoUpdateOperationFactory
    {
        ICallistoUpdate<T> CreateUpdateById<T>(ObjectId id, UpdateDefinition<T> updateDefinition)
            where T : class, IDocumentRoot;

        ICallistoUpdate<T> CreateUpdate<T>(UpdateDefinition<T> updateDefinition, FilterDefinition<T> filterDefinition)
            where T : class, IDocumentRoot;

        ICallistoUpdate<T> CreateUpdate<T>(UpdateDefinition<T> updateDefinition, FilterDefinition<T> filterDefinition, string operationName)
            where T : class, IDocumentRoot;

        ICallistoUpdate<T> CreateUpdate<T>(UpdateDefinition<T> updateDefinition, FilterDefinition<T> filterDefinition,
                                           UpdateOptions updateOptions)
            where T : class, IDocumentRoot;

        /// <summary>
        ///     Create an update command.
        /// </summary>
        /// <param name="operationName">The name of the operation</param>
        /// <param name="filterDefinition">Filter</param>
        /// <param name="updateDefinition">Update</param>
        /// <param name="updateOptions">Options</param>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>
        ///     <see cref="DefaultCallistoUpdate{T}" />
        /// </returns>
        ICallistoUpdate<T> CreateUpdate<T>(UpdateDefinition<T> updateDefinition, FilterDefinition<T> filterDefinition,
                                           UpdateOptions updateOptions, string operationName)
            where T : class, IDocumentRoot;

    }
}