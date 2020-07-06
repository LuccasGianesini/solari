using System.Collections.Generic;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.CQR;

namespace Solari.Callisto.Framework.Factories
{
    public interface ICallistoInsertOperationFactory
    {
        ICallistoInsertOne<T> CreateInsertOne<T>(T value)
            where T : class, IDocumentRoot;

        ICallistoInsertOne<T> CreateInsertOne<T>(T value, string operationName)
            where T : class, IDocumentRoot;

        ICallistoInsertOne<T> CreateInsertOne<T>(T value, InsertOneOptions insertOneOptions)
            where T : class, IDocumentRoot;

        ICallistoInsertOne<T> CreateInsertOne<T>(T value, InsertOneOptions insertOneOptions, string operationName)
            where T : class, IDocumentRoot;

        ICallistoInsertMany<T> CreateInsertMany<T>(IEnumerable<T> values)
            where T : class, IDocumentRoot;

        ICallistoInsertMany<T> CreateInsertMany<T>(IEnumerable<T> values, string operationName)
            where T : class, IDocumentRoot;

        ICallistoInsertMany<T> CreateInsertMany<T>(IEnumerable<T> values, InsertManyOptions insertOneOptions)
            where T : class, IDocumentRoot;

        ICallistoInsertMany<T> CreateInsertMany<T>(IEnumerable<T> values, InsertManyOptions insertOneOptions, string operationName)
            where T : class, IDocumentRoot;
    }
}