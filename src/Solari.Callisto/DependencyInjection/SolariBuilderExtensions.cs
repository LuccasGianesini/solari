using System;
using Microsoft.Extensions.DependencyInjection;
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
        public static ISolariBuilder AddCallisto(this ISolariBuilder solariBuilder, Action<ICallistoConfiguration> configure)
        {
            AddCoreServices(solariBuilder);
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
        public static ISolariBuilder AddCallistoWithDefaults(this ISolariBuilder solariBuilder, Action<ICallistoConfiguration> configure)
        {
            
            AddCoreServices(solariBuilder);
            configure(new CallistoConfiguration(solariBuilder).RegisterDefaultConventionPack().RegisterDefaultClassMaps());
            return solariBuilder;
        }

        private static void AddCoreServices(this ISolariBuilder builder)
        {
            builder.AddCallistoConnector();
            builder.Services.AddTransient<ICallistoUpdateOperationFactory, CallistoUpdateOperationFactory>();
            builder.Services.AddTransient<ICallistoInsertOperationFactory, CallistoInsertOperationFactory>();
            builder.Services.AddTransient<ICallistoDeleteOperationFactory, CallistoDeleteOperationFactory>();
            builder.Services.AddTransient<ICallistoReplaceOperationFactory, CallistoReplaceOperationFactory>();
            builder.Services.AddTransient<ICallistoQueryOperationFactory, CallistoQueryOperationFactory>();
        }
    }
}