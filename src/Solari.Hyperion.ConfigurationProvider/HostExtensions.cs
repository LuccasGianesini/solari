using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Solari.Hyperion.Abstractions;
using Solari.Sol;
using Solari.Sol.Extensions;

namespace Solari.Hyperion.ConfigurationProvider
{
    public static class HostExtensions
    {
        public static IWebHostBuilder UseHyperion(this IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.UseHyperion(null));
            return builder;
        }

        public static IHostBuilder UseHyperion(this IHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.UseHyperion(null));
            return builder;
        }

        public static IWebHostBuilder UseHyperion(this IWebHostBuilder builder, Func<HyperionOptions, HyperionOptions> options)
        {
            builder.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.UseHyperion(options));
            return builder;
        }

        public static IHostBuilder UseHyperion(this IHostBuilder builder, Func<HyperionOptions, HyperionOptions> options)
        {
            builder.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.UseHyperion(options));
            return builder;
        }


        public static IConfigurationBuilder UseHyperion(this IConfigurationBuilder builder, Func<HyperionOptions, HyperionOptions> options)
        {
            IConfigurationRoot configRoot = builder.Build();
            var hype = configRoot.GetOptions<HyperionOptions>(HyperionConstants.AppSettingsSection);
            var app = configRoot.GetOptions<ApplicationOptions>(SolariConstants.ApplicationAppSettingsSection);
            hype ??= options(new HyperionOptions());
            var src = new HyperionConfigurationSource
            {
                Options = hype,
                ApplicationOptions = app
            };
            builder.Add(src);
            return builder;
        }
    }
}