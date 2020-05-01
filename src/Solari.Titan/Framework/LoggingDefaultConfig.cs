using System;
using System.Linq;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Solari.Sol;
using Solari.Titan.Abstractions;

namespace Solari.Titan.Framework
{
    internal static class LoggingDefaultConfig
    {
        private const LogEventLevel OverrideDefault = LogEventLevel.Warning;

        internal static LoggerConfiguration BuildDefaultConfig(LoggerConfiguration config,
                                                               TitanOptions options, ApplicationOptions appOptions, string contentRootPath)
        {
            if (options == null) throw new ArgumentException("Serilog options cannot be null. Check your AppSettings.json and your hosting environment");

            ConfigureMinimumLevels(config, options);
            ConfigureEnrich(config, appOptions);
            AddSinks(config, options, appOptions, contentRootPath);
            return config;
        }

        private static void AddSinks(LoggerConfiguration config, TitanOptions options, ApplicationOptions appOptions, string contentRootPath)
        {
            config
                .ConfigureConsole(options.UseConsole)
                .ConfigureFile(options, contentRootPath)
                .ConfigureElasticSearch(options, appOptions)
                .ConfigureGreyLog(options)
                .ConfigureSeq(options);
        }

        private static void ConfigureEnrich(LoggerConfiguration config, ApplicationOptions appOptions)
        {
            config.Enrich.FromLogContext()
                  .Enrich.WithExceptionDetails()
                  .Enrich.WithThreadId()
                  .Enrich.WithThreadName()
                  .Enrich.WithProperty("Application", appOptions.ApplicationName)
                  .Enrich.WithProperty("Application Version", appOptions.ApplicationVersion)
                  .Enrich.WithProperty("Application Environment", appOptions.ApplicationEnvironment);
        }

        private static void ConfigureMinimumLevels(LoggerConfiguration config, TitanOptions options)
        {
            config
                .MinimumLevel.Is(TitanLibHelper.GetLogLevel(options.DefaultLevel))
                .MinimumLevel.Override("System", TitanLibHelper.GetLogLevel(options.Overrides.System))
                .MinimumLevel.Override("Microsoft", TitanLibHelper.GetLogLevel(options.Overrides.Microsoft))
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", TitanLibHelper.GetLogLevel(options.Overrides.MicrosoftHostingLifetime));
            foreach (string[] item in options.Overrides.Custom.Select(s => s.Split(":")))
            {
                config.MinimumLevel.Override(item[0], TitanLibHelper.GetLogLevel(item[1]));
            }
        }
    }
}