using System;
using System.Text;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.Loki;
using Serilog.Sinks.Loki.gRPC;
using Solari.Sol.Extensions;
using Solari.Titan.Abstractions;

namespace Solari.Titan.Framework
{
    internal static class SinksConfiguration
    {
        internal static LoggerConfiguration ConfigureLoki(this LoggerConfiguration configuration, LokiOptions lokiOptions, string appName,
                                                          string appEnv)
        {
            if (lokiOptions == null)
                return configuration;
            if (lokiOptions.Enabled is false)
                return configuration;

            if (string.IsNullOrEmpty(lokiOptions.RpcEndpoint))
                throw new TitanException("Loki gRPC endpoint is null or empty");
            configuration.WriteTo.LokigRPC(lokiOptions.RpcEndpoint, new LokiLabelProvider(appName, appEnv), 
                                           lokiOptions.Period.ToTimeSpan(), lokiOptions.QueueLimit,
                                           lokiOptions.BatchSizeLimit, TitanLibHelper.GetLogLevel(lokiOptions.LogLevelRestriction), 
                                           lokiOptions.StackTraceAsLabel);

            // configuration.WriteTo.LokiHttp(new NoAuthCredentials(lokiOptions.RpcEndpoint));
            return configuration;
        }

        internal static LoggerConfiguration ConfigureConsole(this LoggerConfiguration configuration, TitanOptions options)
        {
            options.Console ??= new ConsoleOptions();

            if (options.Console.Enabled is false)
                return configuration;

            configuration.WriteTo
                         .Console(outputTemplate: options.Console.OutputTemplate, theme: options.Console.GetConsoleTheme());

            return configuration;
        }

        internal static LoggerConfiguration ConfigureFile(this LoggerConfiguration configuration, FileOptions options, string contentRootPath = "")
        {
            if (options == null || options.Enabled is false) return configuration;

            string path = options.UseContentRoot
                              ? TitanLibHelper.BuildPath(contentRootPath, "logs", ".json")
                              : TitanLibHelper.BuildPath(options.Path, ".json");

            configuration.WriteTo.File(new JsonFormatter(), path,
                                       rollingInterval: TitanLibHelper.GetRollingInterval(options.RollingInterval),
                                       flushToDiskInterval: options.Period.ToTimeSpan(),
                                       rollOnFileSizeLimit: true, encoding: Encoding.UTF8);

            return configuration;
        }
    }
}