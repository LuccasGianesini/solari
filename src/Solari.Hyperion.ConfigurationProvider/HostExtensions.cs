using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Solari.Hyperion.Abstractions;
using Solari.Sol;
using Solari.Sol.Extensions;

namespace Solari.Hyperion.ConfigurationProvider
{
    public static class HostExtensions
    {
        public static IConfigurationBuilder UseHyperion(this IConfigurationBuilder builder, Func<HyperionOptions, HyperionOptions> func)
        {
            IConfigurationRoot configRoot = builder.Build();

            SolariBuilderExtensions.GetHyperionOptions(configRoot, out HyperionOptions options);

            var app = configRoot.GetOptions<ApplicationOptions>(SolariConstants.ApplicationAppSettingsSection);

            var src = new HyperionConfigurationSource
            {
                Options = options,
                ApplicationOptions = app
            };
            builder.Add(src);
            return builder;
        }

        public static IWebHostBuilder UseHyperion(this IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.UseHyperion(null))
                   .ConfigureServices((context, collection) => { AddHyperion(collection, context.Configuration); });
            return builder;
        }

        public static IHostBuilder UseHyperion(this IHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.UseHyperion(null))
                   .ConfigureServices((context, collection) => { AddHyperion(collection, context.Configuration); });
            return builder;
        }

        public static IWebHostBuilder UseHyperion(this IWebHostBuilder builder, Func<HyperionOptions, HyperionOptions> options)
        {
            builder.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.UseHyperion(options))
                   .ConfigureServices((context, collection) => { AddHyperion(collection, context.Configuration); });
            ;
            return builder;
        }

        public static IHostBuilder UseHyperion(this IHostBuilder builder, Func<HyperionOptions, HyperionOptions> options)
        {
            builder.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.UseHyperion(options))
                   .ConfigureServices((context, collection) => { AddHyperion(collection, context.Configuration); });
            return builder;
        }


        private static void AddHyperion(IServiceCollection collection, IConfiguration configuration)
        {
            HyperionOptions options = SolariBuilderExtensions.ConfigureHyperionOptions(collection, configuration);
            SolariBuilderExtensions.AddHyperionCoreServices(collection, options);
            SolariBuilderExtensions.RegisterApplication(collection, options);
        }
    }
}