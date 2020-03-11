using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using Serilog;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Connector;
using Solari.Rhea.Utils;
using Solari.Sol;

namespace Solari.Callisto.Framework
{
    public class CallistoConfiguration : ICallistoConfiguration
    {
        private readonly ISolariBuilder _solariBuilder;

        public CallistoConfiguration(ISolariBuilder solariBuilder) { _solariBuilder = solariBuilder; }

        /// <summary>
        /// Configure the convention pack.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public ICallistoConfiguration ConfigureConventionPack(Func<IConventionPackBuilder, IConventionPack> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            _solariBuilder.AddBuildAction(provider => { builder(new ConventionPackBuilder()); });
            return this;
        }

        /// <summary>
        /// Create and register the default convention pack.
        /// </summary>
        /// <returns></returns>
        public ICallistoConfiguration RegisterDefaultConventionPack()
        {
            ConfigureConventionPack(builder => builder.RegisterConventionPack());
            return this;
        }

        /// <summary>
        /// Read the AppDomain and register the class map for all the classes implementing <see cref="IDocumentRoot"/> and <see cref="IDocumentNode"/>
        /// </summary>
        /// <returns></returns>
        public ICallistoConfiguration RegisterDefaultClassMaps()
        {
            CallistoLogger.ClassMapsLogger.UsingDefaultClassMaps();
            IEnumerable<Type> domain = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToArray();
            RegisterClassMaps(builder => builder.AutoRegister(new AppDomainClasses(GetRoots(domain),
                                                                                   GetNodes(domain))));

            return this;
        }

        /// <summary>
        /// Register class custom class maps..
        /// </summary>
        /// <param name="classMapper">Mapper</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ICallistoConfiguration RegisterClassMaps(Action<ICallistoClassMapper> classMapper)
        {
            if (classMapper == null) throw new ArgumentNullException(nameof(classMapper));
            _solariBuilder.AddBuildAction(provider => { classMapper(new CallistoClassMapper()); });

            return this;
        }

        /// <summary>
        /// Register a MongoDb collection repository.
        /// </summary>
        /// <param name="collectionName">The collection name</param>
        /// <param name="lifetime">Lifetime of the repository service.</param>
        /// <typeparam name="TService">Repository interface</typeparam>
        /// <typeparam name="TImplementation">Repository Implementation</typeparam>
        /// <typeparam name="TEntity">Root document</typeparam>
        /// <returns></returns>
        public ICallistoConfiguration RegisterCollection<TService, TImplementation, TEntity>(string collectionName,
                                                                                             ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TEntity : class, IDocumentRoot
            where TImplementation : CallistoRepository<TEntity>, TService

        {
            _solariBuilder.Services.Add(ServiceDescriptor.Describe(typeof(TService), provider =>
            {
                // ReSharper disable once ConvertToLambdaExpression
                CallistoLogger.CollectionLogger.CallingRepository(collectionName, lifetime.ToString());
                return ActivatorUtilities.CreateInstance<TImplementation>(provider,
                                                                          new CallistoContext(collectionName,
                                                                                              provider.GetRequiredService<ICallistoConnection>(),
                                                                                              provider.GetRequiredService<ICallistoOperationFactory>()));
            }, lifetime));

            return this;
        }

        private static IEnumerable<Type> GetRoots(IEnumerable<Type> domain)
        {
            return domain
                   .Where(x => typeof(IDocumentRoot).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                   .Select(x =>
                   {
                       CallistoLogger.ClassMapsLogger.IdentifiedRoot(x.Name);
                       return x;
                   }).ToList();
            
        }
        private static IEnumerable<Type> GetNodes(IEnumerable<Type> domain)
        {
            return domain
                   .Where(x => typeof(IDocumentNode).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                   .Select(x =>
                   {
                       CallistoLogger.ClassMapsLogger.IdentifiedNode(x.Name);
                       return x;
                   }).ToList();
        }
    }
}