using System;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.CQR;
using Solari.Callisto.Abstractions.Exceptions;

namespace Solari.Callisto.Framework.Factories
{
    public class CallistoReplaceOperationFactory : ICallistoReplaceOperationFactory
    {
        public ICallistoReplace<T> CreateReplaceById<T>(T replacement, Guid id)
            where T : class, IDocumentRoot
        {
            if (id == Guid.Empty)
            {
                throw new CallistoException("An empty ObjectId is invalid. Cannot create ReplaceById operation.");
            }

            return CreateReplace(replacement, Builders<T>.Filter.Eq(a => a.Id, id), null, string.Empty);
        }
        
        public ICallistoReplace<T> CreateReplace<T>(T replacement, FilterDefinition<T> filterDefinition)
            where T : class, IDocumentRoot
        {
            return CreateReplace(replacement, filterDefinition, null, string.Empty);
        }
        
        public ICallistoReplace<T> CreateReplace<T>(T replacement, FilterDefinition<T> filterDefinition, string operationName)
            where T : class, IDocumentRoot
        {
            return CreateReplace(replacement, filterDefinition, null, operationName);
        }

        public ICallistoReplace<T> CreateReplace<T>(T replacement, FilterDefinition<T> filterDefinition,
                                                    ReplaceOptions replaceOptions)
            where T : class, IDocumentRoot
        {
            return CreateReplace(replacement, filterDefinition, replaceOptions, string.Empty);
        }

        public ICallistoReplace<T> CreateReplace<T>(T replacement, FilterDefinition<T> filterDefinition,
                                                    ReplaceOptions replaceOptions, string operationName)
            where T : class, IDocumentRoot
        {
            if (replacement is null)
                throw new CallistoException($"An {nameof(ICallistoReplace<T>)} requires instance of {nameof(T)}");
            if (filterDefinition is null)
                throw new CallistoException($"An {nameof(ICallistoReplace<T>)} requires an {nameof(FilterDefinition<T>)}.");

            return new DefaultCallistoReplace<T>(operationName, replacement, filterDefinition, replaceOptions);
        }
    }
}