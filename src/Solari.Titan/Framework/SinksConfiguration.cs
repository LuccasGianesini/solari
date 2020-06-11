using System.Text;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.Loki;
using Solari.Sol.Extensions;
using Solari.Titan.Abstractions;
using Solari.Titan.Loki;

namespace Solari.Titan.Framework
{
    internal static class SinksConfiguration
    {
        internal static LoggerConfiguration ConfigureLoki(this LoggerConfiguration configuration, LokiOptions lokiOptions)
        {
            if (lokiOptions is null)
                throw new TitanException("Cannot configure Loki sink because LokiOptions is null.");
            if (lokiOptions.Enabled is false)
                return configuration;

            if (string.IsNullOrEmpty(lokiOptions.Endpoint))
                throw new TitanException("Loki cannot be null or empty.");


            LokiCredentials credentials;
            if (lokiOptions.Credentials is null)
            {
                credentials = new NoAuthCredentials(lokiOptions.Endpoint);
            }
            else
            {
                credentials = new BasicAuthCredentials(lokiOptions.Endpoint, lokiOptions.Credentials.Username, lokiOptions.Credentials.Password);
            }

            configuration.WriteTo.TitanLoki(credentials, null, null, TitanLibHelper.GetLogLevel(lokiOptions.LogLevelRestriction),
                                            lokiOptions.BatchSizeLimit, lokiOptions.Period.ToTimeSpan(), lokiOptions.QueueLimit);
            return configuration;
        }

        internal static LoggerConfiguration ConfigureConsole(this LoggerConfiguration configuration, TitanOptions options)
        {
            if (options.Console is null)
                return configuration;
            if (options.Console.Enabled is false)
                return configuration;

            configuration.WriteTo.Console(outputTemplate: options.Console.OutputTemplate,
                                          theme: options.Console.GetConsoleTheme());

            return configuration;
        }

        internal static LoggerConfiguration ConfigureFile(this LoggerConfiguration configuration, FileOptions options,
                                                          string contentRootPath = "")
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
