using System.Text;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.Graylog;
using Serilog.Sinks.Loki.gRPC;
using Solari.Sol.Extensions;
using Solari.Titan.Abstractions;

namespace Solari.Titan.Framework
{
    internal static class SinksConfiguration
    {
        internal static LoggerConfiguration ConfigureLoki(this LoggerConfiguration configuration, LokiOptions lokiOptions)
        {
            if (lokiOptions == null)
                return configuration;
            if (lokiOptions.Enabled is false)
                return configuration;

            if (string.IsNullOrEmpty(lokiOptions.RpcEndpoint))
                throw new TitanException("Loki gRPC endpoint is null or empty");
            configuration.WriteTo.LokigRPC(lokiOptions.RpcEndpoint, null, lokiOptions.Period.ToTimeSpan(), lokiOptions.QueueLimit, lokiOptions
                                               .BatchSizeLimit, TitanLibHelper.GetLogLevel(lokiOptions.LogLevelRestriction), lokiOptions.StackTraceAsLabel);
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

        internal static LoggerConfiguration ConfigureSeq(this LoggerConfiguration configuration, SeqOptions options)
        {
            if (options == null || options.Enabled is false) return configuration;

            long batchPosting = options.RawIngestionPayload / options.EventBodySizeLimit;

            configuration.WriteTo.Seq(options.IngestionEndpoint,
                                      TitanLibHelper.GetLogLevel(options.LogLevelRestriction),
                                      period: options.Period.ToTimeSpan(), apiKey: options.Apikey, compact: true,
                                      eventBodyLimitBytes: options.EventBodySizeLimit, batchPostingLimit: (int) batchPosting,
                                      queueSizeLimit: options.QueueSizeLimit);

            return configuration;
        }

        internal static LoggerConfiguration ConfigureGrayLog(this LoggerConfiguration configuration, GrayLogOptions options)
        {
            var sinkOptions = new GraylogSinkOptions
            {
                Facility = options.Facility,
                Port = options.Port,
                TransportType = options.GetTransportType(),
                HostnameOrAddress = options.Address,
                StackTraceDepth = options.StackTraceDepth,
                MaxMessageSizeInUdp = options.MaxMessageSizeInUdp,
                ShortMessageMaxLength = options.ShortMessageMaxLength,
                MinimumLogEventLevel = TitanLibHelper.GetLogLevel(options.LogLevelRestriction),
                MessageGeneratorType = options.GetMessageIdGeneratorType()
            };
            return options == null || options.Enabled is false ? configuration : configuration.WriteTo.Graylog(sinkOptions);
        }
    }
}