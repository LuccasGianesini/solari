using System.Linq;
using Serilog;
using Serilog.Core;
using Serilog.Exceptions;
using Solari.Sol;
using Solari.Sol.Abstractions;
using Solari.Titan.Abstractions;

namespace Solari.Titan.Framework
{
    internal static class LoggingDefaultConfig
    {
        internal static LoggerConfiguration BuildDefaultConfig(LoggerConfiguration config,
                                                               TitanOptions options, ApplicationOptions appOptions, string contentRootPath)
        {
            ConfigureMinimumLevels(config, options);
            ConfigureEnrich(config, appOptions);
            AddSinks(config, options, contentRootPath);
            return config;
        }

        private static void AddSinks(LoggerConfiguration config, TitanOptions options, string contentRootPath)
        {
            config
                .ConfigureConsole(options)
                .ConfigureFile(options.File, contentRootPath)
                .ConfigureLoki(options.Loki, options.DynamicDefaultLevel)
                .ConfigureGrayLog(options.Graylog, options.DynamicDefaultLevel);
        }

        private static void ConfigureEnrich(LoggerConfiguration config, ApplicationOptions appOptions)
        {
            config.Enrich.FromLogContext()
                  .Enrich.WithExceptionDetails()
                  .Enrich.With<JaegerEnricher>()
                  .Enrich.WithProperty("project", appOptions.Project)
                  .Enrich.WithProperty("app", appOptions.ApplicationName)
                  .Enrich.WithProperty("version", appOptions.ApplicationVersion)
                  .Enrich.WithProperty("env", appOptions.ApplicationEnvironment);
        }

        private static void ConfigureMinimumLevels(LoggerConfiguration config, TitanOptions options)
        {
            if (options.DynamicDefaultLevel)
                config.MinimumLevel.ControlledBy(new LoggingLevelSwitch(TitanLibHelper.GetLogLevel(options.DefaultLevel)));
            else
                config.MinimumLevel.Is(TitanLibHelper.GetLogLevel(options.DefaultLevel));

            config
                .MinimumLevel.Override("System", TitanLibHelper.GetLogLevel(options.Overrides.System))
                .MinimumLevel.Override("Microsoft", TitanLibHelper.GetLogLevel(options.Overrides.Microsoft))
                .MinimumLevel.Override("Microsoft.AspNetCore", TitanLibHelper.GetLogLevel(options.Overrides.AspNetCore))
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", TitanLibHelper.GetLogLevel(options.Overrides.MicrosoftHostingLifetime));

            foreach (string[] item in options.Overrides.Custom.Select(s => s.Split(":")))
                config.MinimumLevel.Override(item[0], TitanLibHelper.GetLogLevel(item[1]));
        }
    }
}
