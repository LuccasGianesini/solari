using System;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.CQR;
using Solari.Callisto.Abstractions.Exceptions;

namespace Solari.Callisto.Framework.Factories
{
    public class CallistoUpdateOperationFactory : ICallistoUpdateOperationFactory
    {
        
        public ICallistoUpdate<T> CreateUpdateById<T>(Guid id, UpdateDefinition<T> updateDefinition)
            where T : class, IDocumentRoot
        {
            if (id == Guid.Empty)
            {
                throw new CallistoException("An empty ObjectId is invalid. Cannot create UpdateById operation.");
            }

            return CreateUpdate(updateDefinition, Builders<T>.Filter.Eq(a => a.Id, id), null, string.Empty);
        }

        public ICallistoUpdate<T> CreateUpdate<T>(UpdateDefinition<T> updateDefinition, FilterDefinition<T> filterDefinition)
            where T : class, IDocumentRoot
        {
            return CreateUpdate(updateDefinition, filterDefinition, null, string.Empty);
        }

        public ICallistoUpdate<T> CreateUpdate<T>(UpdateDefinition<T> updateDefinition, FilterDefinition<T> filterDefinition, string operationName)
            where T : class, IDocumentRoot
        {
            return CreateUpdate(updateDefinition, filterDefinition, null, operationName);
        }

        public ICallistoUpdate<T> CreateUpdate<T>(UpdateDefinition<T> updateDefinition, FilterDefinition<T> filterDefinition,
                                                  UpdateOptions updateOptions)
            where T : class, IDocumentRoot
        {
            return CreateUpdate(updateDefinition, filterDefinition, updateOptions, string.Empty);
        }

        /// <summary>
        ///     Create an update command.
        /// </summary>
        /// <param name="operationName">The name of the operation</param>
        /// <param name="filterDefinition">Filter</param>
        /// <param name="updateDefinition">Update</param>
        /// <param name="updateOptions">Options</param>
        /// <typeparam name="T">Entity type</typeparam>
        /// <exception cref="CallistoException">When the filter definition or the update definition are null.</exception>
        /// <returns>
        ///     <see cref="DefaultCallistoUpdate{T}" />
        /// </returns>
        public ICallistoUpdate<T> CreateUpdate<T>(UpdateDefinition<T> updateDefinition, FilterDefinition<T> filterDefinition,
                                                  UpdateOptions updateOptions, string operationName)
            where T : class, IDocumentRoot
        {
            if (updateDefinition is null)
                throw new CallistoException($"An {nameof(ICallistoUpdate<T>)} requires an {nameof(UpdateDefinition<T>)}.");
            if (filterDefinition is null)
                throw new CallistoException($"An {nameof(ICallistoUpdate<T>)} requires an {nameof(FilterDefinition<T>)}.");

            return new DefaultCallistoUpdate<T>(operationName, updateDefinition, filterDefinition, updateOptions);
        }
    }
}