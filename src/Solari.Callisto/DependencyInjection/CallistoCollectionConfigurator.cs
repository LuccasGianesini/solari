using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Callisto.Framework;
using Solari.Callisto.Framework.Factories;
using Solari.Callisto.Framework.Operators;
using Solari.Sol;

// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local

namespace Solari.Callisto.DependencyInjection
{
    public class CallistoCollectionConfigurator : ICallistoCollectionConfigurator
    {
        private readonly ISolariBuilder _solariBuilder;
        private readonly string _clientName;

        public CallistoCollectionConfigurator(ISolariBuilder solariBuilder, string clientName)
        {
            _solariBuilder = solariBuilder;
            _clientName = clientName;
        }

        public ICallistoCollectionConfigurator ConfigureCollection<TInterface, TConcrete, TDocumentRoot>(string database,
                                                                                                         string collection,
                                                                                                         ServiceLifetime lifetime)
            where TDocumentRoot : class, IDocumentRoot
            where TConcrete : CallistoCollection<TDocumentRoot>, TInterface

        {
            _solariBuilder.Services.Add(ServiceDescriptor.Describe(typeof(TInterface), provider =>
            {
                ValidateCollectionRegistration<TInterface, TConcrete, TDocumentRoot>(collection, database);


                CallistoLogger.CollectionLogger.CallingRepository(collection, lifetime.ToString());

                var registry = provider.GetRequiredService<ICallistoClientRegistry>();
                ICallistoClient callistoClient = registry.GetClient(_clientName);
                IMongoDatabase mongoDatabase = callistoClient.MongoClient.GetDatabase(database);
                IMongoCollection<TDocumentRoot> mongoCollection = mongoDatabase.GetCollection<TDocumentRoot>(collection);


                CollectionOperators<TDocumentRoot> operators =
                    CreateCollectionOperators<TInterface, TConcrete, TDocumentRoot>(mongoCollection, provider);

                var context = new CallistoCollectionContext<TDocumentRoot>(callistoClient, mongoCollection, mongoDatabase);

                return ActivatorUtilities.CreateInstance<TConcrete>(provider, context, operators);
            }, lifetime));

            return this;
        }

        public ICallistoCollectionConfigurator ConfigureScopedCollection<TInterface, TConcrete, TDocumentRoot>(string database,
                                                                                                               string collection)
            where TDocumentRoot : class, IDocumentRoot
            where TConcrete : CallistoCollection<TDocumentRoot>, TInterface
        {
            return ConfigureCollection<TInterface, TConcrete, TDocumentRoot>(database, collection, ServiceLifetime.Scoped);
        }

        public ICallistoCollectionConfigurator ConfigureTransientCollection<TService, TConcrete, TCollection>(string database,
                                                                                                                    string collection)
            where TCollection : class, IDocumentRoot
            where TConcrete : CallistoCollection<TCollection>, TService
        {
            return ConfigureCollection<TService, TConcrete, TCollection>(database, collection, ServiceLifetime.Transient);
        }

        public ICallistoCollectionConfigurator ConfigureSingletonCollection<TService, TConcrete, TCollection>(string database,
                                                                                                                    string collection)
            where TCollection : class, IDocumentRoot
            where TConcrete : CallistoCollection<TCollection>, TService
        {
            return ConfigureCollection<TService, TConcrete, TCollection>(database, collection,
                                                                               ServiceLifetime.Singleton);
        }

        // private static IEnumerable<Type> GetRoots(IEnumerable<Type> domain)
        // {
        //     return domain
        //            .Where(x => CallistoTypeSelector.IsDocumentRoot(x) && !x.IsInterface && !x.IsAbstract)
        //            .Select(x =>
        //            {
        //                CallistoLogger.ClassMapsLogger.IdentifiedRoot(x.Name);
        //                return x;
        //            }).ToList();
        // }
        //
        // private static IEnumerable<Type> GetNodes(IEnumerable<Type> domain)
        // {
        //     return domain
        //            .Where(x => CallistoTypeSelector.IsDocumentNode(x) && !x.IsInterface && !x.IsAbstract)
        //            .Select(x =>
        //            {
        //                CallistoLogger.ClassMapsLogger.IdentifiedNode(x.Name);
        //                return x;
        //            }).ToList();
        // }

        private static void ValidateCollectionRegistration<TInterface, TConcrete, TDocumentRoot>(string collection, string database)
            where TDocumentRoot : class, IDocumentRoot where TConcrete : CallistoCollection<TDocumentRoot>, TInterface
        {
            if (string.IsNullOrEmpty(collection))
                throw new
                    CallistoException($"Unable to create an instance of '{typeof(TInterface).Name}' without a collection being specified.");

            if (string.IsNullOrEmpty(database))
                throw new
                    CallistoException($"Unable to create an instance of '{typeof(TInterface).Name}' without a database being specified.");
        }

        private static CollectionOperators<TDocumentRoot> CreateCollectionOperators<TInterface, TConcrete, TDocumentRoot>(
            IMongoCollection<TDocumentRoot> mongoCollection, IServiceProvider provider)
            where TDocumentRoot : class, IDocumentRoot
            where TConcrete : CallistoCollection<TDocumentRoot>, TInterface
        {
            var deleteOperator =
                new DeleteOperator<TDocumentRoot>(mongoCollection, provider.GetRequiredService<ICallistoDeleteOperationFactory>());
            var replaceOperator =
                new ReplaceOperator<TDocumentRoot>(mongoCollection, provider.GetRequiredService<ICallistoReplaceOperationFactory>());
            var queryOperator =
                new QueryOperator<TDocumentRoot>(mongoCollection, provider.GetRequiredService<ICallistoQueryOperationFactory>());
            var updateOperator =
                new UpdateOperator<TDocumentRoot>(mongoCollection, provider.GetRequiredService<ICallistoUpdateOperationFactory>());
            var insertOperator =
                new InsertOperator<TDocumentRoot>(mongoCollection, provider.GetRequiredService<ICallistoInsertOperationFactory>());

            var operators = new CollectionOperators<TDocumentRoot>(deleteOperator, replaceOperator,
                                                                   insertOperator, queryOperator,
                                                                   updateOperator);
            return operators;
        }
    }
}
