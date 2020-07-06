using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts;
using Solari.Callisto.Connector.DependencyInjection;
using Solari.Callisto.Framework;
using Solari.Callisto.Framework.Factories;
using Solari.Sol;
using Solari.Sol.Extensions;

namespace Solari.Callisto.DependencyInjection
{
    public static class SolariBuilderExtensions
    {
        public static ISolariBuilder AddCallisto(this ISolariBuilder builder,
                                                 Action<ICallistoConventionRegistry> conventionPackAction,
                                                 Action<ICallistoClassMapper> classMapAction,
                                                 Action<ICallistoClientConfigurator> configure,
                                                 ServiceLifetime factoriesLifetime)
        {
            AddCoreServices(builder, factoriesLifetime);
            classMapAction?.Invoke(new CallistoClassMapper());
            conventionPackAction.Invoke(new CallistoConventionRegistry());


            IConfigurationSection section = builder.Configuration.GetSection(CallistoConstants.ConnectorAppSettingsSection);
            var options = builder.Configuration.GetOptions<List<CallistoConnectorOptions>>(section);
            configure?.Invoke(new CallistoClientConfigurator(builder, options));
            return builder;
        }

        /// <summary>
        /// Configures the library to use default class maps and conventions.
        /// It also adds the callisto connector.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static ISolariBuilder AddCallistoWithDefaults(this ISolariBuilder builder,
                                                             Action<ICallistoClientConfigurator> configure,
                                                             ServiceLifetime factoriesLifetime)
        {
            builder.AddCallisto(registry => registry.AddDefaultConventions().RegisterConventionPack(),
                                mapper => mapper.RegisterClassMaps(GetCallistoTypes(ReadAppDomain())),
                                configure, factoriesLifetime);
            return builder;
        }

        private static void AddCoreServices(this ISolariBuilder builder, ServiceLifetime factoriesLifetime)
        {
            builder.AddConnectorCoreServices();
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

        private static IEnumerable<Type> GetCallistoTypes(IEnumerable<Type> appDomain)
        {
            return appDomain.Where(x => (CallistoTypeSelector.IsDocumentRoot(x) || CallistoTypeSelector.IsDocumentNode(x)) &&
                                        !x.IsInterface && !x.IsAbstract)
                            .Select(x =>
                            {
                                CallistoLogger.ClassMapsLogger.IdentifiedType(x.Name);
                                return x;
                            });
        }

        private static IEnumerable<Type> ReadAppDomain()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToArray();
        }
    }
}
