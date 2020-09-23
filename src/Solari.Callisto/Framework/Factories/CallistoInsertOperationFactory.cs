using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.CQR;
using Solari.Callisto.Abstractions.Exceptions;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeMadeStatic.Global

namespace Solari.Callisto.Framework.Factories
{
    public class CallistoInsertOperationFactory : ICallistoInsertOperationFactory
    {
        public ICallistoInsertOne<T> CreateInsertOne<T>(T value)
            where T : class, IDocumentRoot
        {
            return CreateInsertOne(value, null, string.Empty);
        }

        public ICallistoInsertOne<T> CreateInsertOne<T>(T value, string operationName)
            where T : class, IDocumentRoot
        {
            return CreateInsertOne(value, null, operationName);
        }

        public ICallistoInsertOne<T> CreateInsertOne<T>(T value, InsertOneOptions insertOneOptions)
            where T : class, IDocumentRoot
        {
            return CreateInsertOne(value, insertOneOptions, string.Empty);
        }

        public ICallistoInsertOne<T> CreateInsertOne<T>(T value, InsertOneOptions insertOneOptions, string operationName)
            where T : class, IDocumentRoot
        {
            // if (value is null)
            //     throw new CallistoException($"No insert can be made with a null {nameof(T)} instance");
            return new DefaultCallistoInsertOne<T>(operationName, value, insertOneOptions);
        }

        public ICallistoInsertMany<T> CreateInsertMany<T>(IEnumerable<T> values)
            where T : class, IDocumentRoot
        {
            return CreateInsertMany(values, null, string.Empty);
        }

        public ICallistoInsertMany<T> CreateInsertMany<T>(IEnumerable<T> values, string operationName)
            where T : class, IDocumentRoot
        {
            return CreateInsertMany(values, null, operationName);
        }

        public ICallistoInsertMany<T> CreateInsertMany<T>(IEnumerable<T> values, InsertManyOptions insertOneOptions)
            where T : class, IDocumentRoot
        {
            return CreateInsertMany(values, insertOneOptions, string.Empty);
        }

        public  ICallistoInsertMany<T> CreateInsertMany<T>(IEnumerable<T> values, InsertManyOptions insertOneOptions, string operationName)
            where T : class, IDocumentRoot
        {
            // if (values is null)
            //     throw new CallistoException($"No insert can be made with a null {nameof(T)} instance");
            // if (!values.Any())
            //     throw new CallistoException("There is no need to insert an empty list of values into the database.");
            return new DefaultCallistoInsertMany<T>(operationName, values, insertOneOptions);
        }
    }
}
