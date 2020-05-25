using System;
using MongoDB.Bson;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.CQR;

namespace Solari.Callisto.Framework
{
    public interface ICallistoReplaceOperationFactory
    {
        ICallistoReplace<T> CreateReplaceById<T>(T replacement, ObjectId id)
            where T : class, IDocumentRoot;

        ICallistoReplace<T> CreateReplace<T>(T replacement, FilterDefinition<T> filterDefinition)
            where T : class, IDocumentRoot;

        ICallistoReplace<T> CreateReplace<T>(T replacement, FilterDefinition<T> filterDefinition, string operationName)
            where T : class, IDocumentRoot;

        ICallistoReplace<T> CreateReplace<T>(T replacement, FilterDefinition<T> filterDefinition,
                                             ReplaceOptions replaceOptions)
            where T : class, IDocumentRoot;

        ICallistoReplace<T> CreateReplace<T>(T replacement, FilterDefinition<T> filterDefinition,
                                             ReplaceOptions replaceOptions, string operationName)
            where T : class, IDocumentRoot;
    }
}