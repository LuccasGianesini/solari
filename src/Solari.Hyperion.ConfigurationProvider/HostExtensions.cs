using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Solari.Hyperion.Abstractions;
using Solari.Sol;
using Solari.Sol.Abstractions;
using Solari.Sol.Abstractions.Extensions;

namespace Solari.Hyperion.ConfigurationProvider
{
    public static class HostExtensions
    {
        private static IConfigurationBuilder UseHyperion(this IConfigurationBuilder builder, Func<HyperionOptions, HyperionOptions> func)
        {
            IConfigurationRoot configRoot = builder.Build();

            SolariBuilderExtensions.GetHyperionOptions(configRoot, out HyperionOptions options);

            if (options.ConfigurationProvider is null || !options.ConfigurationProvider.Enabled)
                return builder;

            ApplicationOptions app = configRoot.GetApplicationOptions();

            var src = new HyperionConfigurationSource
            {
                Options = options,
                ApplicationOptions = app
            };
            builder.Add(src);
            return builder;
        }

        public static IWebHostBuilder UseHyperion(this IWebHostBuilder builder, bool addHyperionServices)
        {
            builder.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.UseHyperion(null))
                   .ConfigureServices((context, collection) => { AddHyperion(collection, context.Configuration, addHyperionServices); });
            return builder;
        }

        public static IHostBuilder UseHyperion(this IHostBuilder builder, bool addHyperionServices)
        {
            builder.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.UseHyperion(null))
                   .ConfigureServices((context, collection) => { AddHyperion(collection, context.Configuration, addHyperionServices); });
            return builder;
        }

        public static IWebHostBuilder UseHyperion(this IWebHostBuilder builder, Func<HyperionOptions, HyperionOptions> options, bool addHyperionServices)
        {
            builder.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.UseHyperion(options))
                   .ConfigureServices((context, collection) => { AddHyperion(collection, context.Configuration, addHyperionServices); });
            ;
            return builder;
        }

        public static IHostBuilder UseHyperion(this IHostBuilder builder, Func<HyperionOptions, HyperionOptions> options, bool addHyperionServices)
        {
            builder.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.UseHyperion(options))
                   .ConfigureServices((context, collection) => { AddHyperion(collection, context.Configuration, addHyperionServices); });
            return builder;
        }


        private static void AddHyperion(IServiceCollection collection, IConfiguration configuration, bool addHyperionServices)
        {
            if(!addHyperionServices)
                return;
            HyperionOptions options = SolariBuilderExtensions.ConfigureHyperionOptions(collection, configuration);
            SolariBuilderExtensions.AddHyperionCoreServices(collection, options);
            SolariBuilderExtensions.RegisterApplication(collection, options);
        }
    }
}
