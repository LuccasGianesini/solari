using System;
using MongoDB.Bson;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.CQR;

namespace Solari.Callisto.Framework
{
    public interface ICallistoDeleteOperationFactory
    {
        ICallistoDelete<T> CreateDeleteById<T>(ObjectId id) 
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