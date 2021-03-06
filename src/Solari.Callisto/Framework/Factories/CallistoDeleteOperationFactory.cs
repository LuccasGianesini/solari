using System;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.CQR;
using Solari.Callisto.Abstractions.Exceptions;

namespace Solari.Callisto.Framework.Factories
{
    public class CallistoDeleteOperationFactory : ICallistoDeleteOperationFactory
    {
        public ICallistoDelete<T> CreateDeleteById<T>(Guid id) where T : class, IDocumentRoot
        {
            return CreateDelete(Builders<T>.Filter.Eq(a => a.Id, id));
        }

        public ICallistoDelete<T> CreateDelete<T>(FilterDefinition<T> filterDefinition)
            where T : class, IDocumentRoot
        {
            return CreateDelete(filterDefinition, default, string.Empty);
        }

        public ICallistoDelete<T> CreateDelete<T>(FilterDefinition<T> filterDefinition, string operationName)
            where T : class, IDocumentRoot
        {
            return CreateDelete(filterDefinition, default, operationName);
        }

        public ICallistoDelete<T> CreateDelete<T>(FilterDefinition<T> filterDefinition, DeleteOptions deleteOptions)
            where T : class, IDocumentRoot
        {
            return CreateDelete(filterDefinition, deleteOptions, string.Empty);
        }

        public ICallistoDelete<T> CreateDelete<T>(FilterDefinition<T> filterDefinition, DeleteOptions deleteOptions, string operationName)
            where T : class, IDocumentRoot
        {
            return new DefaultCallistoDelete<T>(operationName, filterDefinition, deleteOptions);
        }
    }
}
