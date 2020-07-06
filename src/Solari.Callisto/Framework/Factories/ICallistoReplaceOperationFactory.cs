using System;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.CQR;

namespace Solari.Callisto.Framework.Factories
{
    public interface ICallistoReplaceOperationFactory
    {
        ICallistoReplace<T> CreateReplaceById<T>(T replacement, Guid id)
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