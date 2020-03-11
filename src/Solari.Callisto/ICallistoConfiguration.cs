using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using Solari.Callisto.Abstractions;

namespace Solari.Callisto
{
    public interface ICallistoConfiguration
    {
        /// <summary>
        /// Configure the convention pack.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        ICallistoConfiguration ConfigureConventionPack(Func<IConventionPackBuilder, IConventionPack> builder);

        /// <summary>
        /// Create and register the default convention pack.
        /// </summary>
        /// <returns></returns>
        ICallistoConfiguration RegisterDefaultConventionPack();

        /// <summary>
        /// Read the AppDomain and register the class map for all the classes implementing <see cref="IDocumentRoot"/> and <see cref="IDocumentNode"/>
        /// </summary>
        /// <returns></returns>
        ICallistoConfiguration RegisterDefaultClassMaps();

        ICallistoConfiguration RegisterCollection<TRepositoryService, TRepositoryImplementation, TEntity>
            (string collectionName, ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TEntity : class, IDocumentRoot
            where TRepositoryImplementation : CallistoRepository<TEntity>, TRepositoryService;
    }
}