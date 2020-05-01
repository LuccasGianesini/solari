using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;
using Solari.Sol;
using Solari.Sol.Extensions;
using Solari.Titan.Abstractions;
using Solari.Titan.Framework;

namespace Solari.Titan.DependencyInjection
{
    public static class ApplicationBuilderExtensions
    {
        public static IWebHostBuilder UseTitan(this IWebHostBuilder webHostBuilder)
        {
            return webHostBuilder.ConfigureServices((collection =>
                                                        {
                                                            collection.TryAdd(ServiceDescriptor.Singleton(typeof(ILogEnricher<>), typeof(LogEnricher<>)));
                                                            collection.TryAdd(ServiceDescriptor.Singleton(typeof(ITitanLogger<>), typeof(TitanLogger<>)));
                                                        }))
                                 .UseSerilog((context, config) =>
                                 {
                                     LoggingDefaultAction(config, GetOptions(context.Configuration),
                                                          GetApplicationOptions(context.Configuration),
                                                          context.HostingEnvironment.ContentRootPath);
                                 });
        }

        public static IHostBuilder UseTitan(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices((collection =>
                                                     {
                                                         collection.TryAdd(ServiceDescriptor.Singleton(typeof(ILogEnricher<>), typeof(LogEnricher<>)));
                                                         collection.TryAdd(ServiceDescriptor.Singleton(typeof(ITitanLogger<>), typeof(TitanLogger<>)));
                                                     }))
                              .UseSerilog((context, config) =>
                              {
                                  LoggingDefaultAction(config, GetOptions(context.Configuration),
                                                       GetApplicationOptions(context.Configuration),
                                                       context.HostingEnvironment.ContentRootPath);
                              });
        }

        private static ApplicationOptions GetApplicationOptions(IConfiguration configuration)
        {
            return configuration.GetOptions<ApplicationOptions>(SolariConstants.ApplicationAppSettingsSection);
        }


        private static TitanOptions GetOptions(IConfiguration configuration)
        {
            return configuration.GetOptions<TitanOptions>(TitanConstants.TitanAppSettingsSection);
        }


        private static LoggerConfiguration LoggingDefaultAction(LoggerConfiguration loggerConfiguration, TitanOptions options, ApplicationOptions
                                                                    appOptions, string contentRootPath)
        {
            return LoggingDefaultConfig.BuildDefaultConfig(loggerConfiguration, options, appOptions, contentRootPath);
        }
    }
}