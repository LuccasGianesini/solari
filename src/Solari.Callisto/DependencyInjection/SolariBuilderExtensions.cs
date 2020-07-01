using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Connector.DependencyInjection;
using Solari.Callisto.Framework;
using Solari.Sol;

namespace Solari.Callisto.DependencyInjection
{
    public static class SolariBuilderExtensions
    {
        /// <summary>
        ///     Add callisto and callisto connector into the DI container.
        /// </summary>
        /// <param name="solariBuilder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static ISolariBuilder AddCallisto(this ISolariBuilder solariBuilder, ServiceLifetime factoriesLifetime,
                                                 Action<ICallistoConfiguration> configure)
        {
            AddCoreServices(solariBuilder, factoriesLifetime);
            configure(new CallistoConfiguration(solariBuilder));
            return solariBuilder;
        }


        /// <summary>
        /// Configures the library to use default class maps and conventions.
        /// It also adds the callisto connector.
        /// </summary>
        /// <param name="solariBuilder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static ISolariBuilder AddCallistoWithDefaults(this ISolariBuilder solariBuilder,
                                                             ServiceLifetime factoriesLifetime,
                                                             Action<ICallistoConfiguration> configure)
        {
            AddCoreServices(solariBuilder, factoriesLifetime);
            configure(new CallistoConfiguration(solariBuilder).RegisterDefaultConventionPack().RegisterDefaultClassMaps());
            return solariBuilder;
        }

        private static void AddCoreServices(this ISolariBuilder builder,
                                            ServiceLifetime factoriesLifetime)
        {
            builder.AddCallistoConnector();
            AddFactories(builder.Services, factoriesLifetime);
        }

        private static void AddFactories(IServiceCollection services, ServiceLifetime lifetime)
        {
            services.Add(ServiceDescriptor.Describe(typeof(ICallistoUpdateOperationFactory),
                                                    provider => new CallistoUpdateOperationFactory(), lifetime));
            services.Add(ServiceDescriptor.Describe(typeof(ICallistoInsertOperationFactory),
                                                    provider => new CallistoInsertOperationFactory(), lifetime));
            services.Add(ServiceDescriptor.Describe(typeof(ICallistoDeleteOperationFactory),
                                                    provider => new CallistoDeleteOperationFactory(), lifetime));
            services.Add(ServiceDescriptor.Describe(typeof(ICallistoReplaceOperationFactory),
                                                    provider => new CallistoReplaceOperationFactory(), lifetime));
            services.Add(ServiceDescriptor.Describe(typeof(ICallistoQueryOperationFactory),
                                                    provider => new CallistoQueryOperationFactory(), lifetime));
        }
    }
}
