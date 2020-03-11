using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Solari.Io;
using Solari.Sol;
using Solari.Titan.Abstractions;
using Solari.Titan.Framework;

namespace Solari.Titan.DependencyInjection
{
    public static class ApplicationBuilderExtensions
    {
        public static IWebHostBuilder UseTitan(this IWebHostBuilder webHostBuilder)
        {
            return webHostBuilder.UseSerilog((context, config) =>
            {
                LoggingDefaultAction(config, GetOptions(context.Configuration),
                                     GetApplicationOptions(context.Configuration),
                                     context.HostingEnvironment.ContentRootPath);
            });
        }

        public static IHostBuilder UseTitan(this IHostBuilder hostBuilder)
        {
            return hostBuilder.UseSerilog((context, config) =>
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


        private static SerilogOptions GetOptions(IConfiguration configuration)
        {
            return configuration.GetOptions<SerilogOptions>(TitanConstants.TitanAppSettingsSection);
        }


        private static LoggerConfiguration LoggingDefaultAction(LoggerConfiguration loggerConfiguration, SerilogOptions options, ApplicationOptions
                                                                    appOptions, string contentRootPath)
        {
            return LoggingDefaultConfig.BuildDefaultConfig(loggerConfiguration, options, appOptions, contentRootPath);
        }
    }
}