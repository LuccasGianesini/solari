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
            return webHostBuilder.UseSerilog((context, config) =>
            {
                ApplicationOptions appOptions = GetApplicationOptions(context.Configuration);
                TitanOptions options = GetOptions(context.Configuration);
                LoggingDefaultConfig.BuildDefaultConfig(config, options, appOptions, AppDomain.CurrentDomain.BaseDirectory);
            });
        }

        public static IHostBuilder UseTitan(this IHostBuilder hostBuilder)
        {
            return hostBuilder.UseSerilog((context, config) =>
            {
                ApplicationOptions appOptions = GetApplicationOptions(context.Configuration);
                TitanOptions options = GetOptions(context.Configuration);
                LoggingDefaultConfig.BuildDefaultConfig(config, options, appOptions, AppDomain.CurrentDomain.BaseDirectory);
            });
        }

        private static ApplicationOptions GetApplicationOptions(IConfiguration configuration)
        {
            IConfigurationSection section = configuration.GetSection(SolariConstants.ApplicationAppSettingsSection);
            return configuration.GetOptions<ApplicationOptions>(section) ?? new ApplicationOptions();
        }


        private static TitanOptions GetOptions(IConfiguration configuration)
        {
            IConfigurationSection section = configuration.GetSection(TitanConstants.TitanAppSettingsSection);
            return configuration.GetOptions<TitanOptions>(section) ?? new TitanOptions();
        }
    }
}