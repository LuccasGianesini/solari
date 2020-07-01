using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using Solari.Callisto.Abstractions;

namespace Solari.Callisto
{
    public interface ICallistoConfiguration
    {

        /// <summary>
        ///     Configure the convention pack.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        ICallistoConfiguration ConfigureConventionPack(Func<IConventionPackBuilder, IConventionPack> builder);

        /// <summary>
        ///     Create and register the default convention pack.
        /// </summary>
        /// <returns></returns>
        ICallistoConfiguration RegisterDefaultConventionPack();

        /// <summary>
        ///     Read the AppDomain and register the class map for all the classes implementing <see cref="IDocumentRoot" /> and
        ///     <see cref="IDocumentNode" />
        /// </summary>
        /// <returns></returns>
        ICallistoConfiguration RegisterDefaultClassMaps();

        ICallistoConfiguration RegisterCollection<TRepositoryService, TRepositoryImplementation, TEntity>(
            string collectionName, ServiceLifetime lifetime)
            where TEntity : class, IDocumentRoot
            where TRepositoryImplementation : CallistoRepository<TEntity>, TRepositoryService;

        ICallistoConfiguration RegisterScopedCollection<TService, TImplementation, TCollection>(string collectionName)
            where TCollection : class, IDocumentRoot
            where TImplementation : CallistoRepository<TCollection>, TService;

        ICallistoConfiguration RegisterTransientCollection<TService, TImplementation, TCollection>(string collectionName)
            where TCollection : class, IDocumentRoot
            where TImplementation : CallistoRepository<TCollection>, TService;

        ICallistoConfiguration RegisterSingletonCollection<TService, TImplementation, TCollection>(string collectionName)
            where TCollection : class, IDocumentRoot
            where TImplementation : CallistoRepository<TCollection>, TService;

        /// <summary>
        ///     Register class custom class maps..
        /// </summary>
        /// <param name="classMapper">Mapper</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        ICallistoConfiguration RegisterClassMaps(Action<ICallistoClassMapper> classMapper);
    }
}
