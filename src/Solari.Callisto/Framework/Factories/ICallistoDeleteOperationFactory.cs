using System;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.CQR;

namespace Solari.Callisto.Framework.Factories
{
    public interface ICallistoDeleteOperationFactory
    {
        ICallistoDelete<T> CreateDeleteById<T>(Guid id) 
            where T : class, IDocumentRoot;
        ICallistoDelete<T> CreateDelete<T>(FilterDefinition<T> filterDefinition)
            where T : class, IDocumentRoot;

        ICallistoDelete<T> CreateDelete<T>(FilterDefinition<T> filterDefinition, string operationName)
            where T : class, IDocumentRoot;

        ICallistoDelete<T> CreateDelete<T>(FilterDefinition<T> filterDefinition, DeleteOptions deleteOptions)
            where T : class, IDocumentRoot;

        ICallistoDelete<T> CreateDelete<T>(FilterDefinition<T> filterDefinition, DeleteOptions deleteOptions, string operationName)
            where T : class, IDocumentRoot;
    }
}