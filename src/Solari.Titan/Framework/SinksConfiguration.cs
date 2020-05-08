using System.Text;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.Graylog;
using Solari.Sol.Extensions;
using Solari.Titan.Abstractions;

namespace Solari.Titan.Framework
{
    //TODO REVIEW ALL METHODS TO COMPLY WITH DEFAULT CONFIG
    internal static class SinksConfiguration
    {
        
        internal static LoggerConfiguration ConfigureConsole(this LoggerConfiguration configuration, TitanOptions options)
        {
            if (options.Console == null)
            {
                options.Console = new ConsoleOptions();
            }
            if (options.Console.Enabled == false)
                return configuration;

            configuration.WriteTo
                         .Console(outputTemplate: options.Console.OutputTemplate, theme: options.Console.GetConsoleTheme());

            return configuration;
        }

        internal static LoggerConfiguration ConfigureFile(this LoggerConfiguration configuration, TitanOptions options, string contentRootPath = "")
        {
            if (options.File == null || options.File.Enabled == false) return configuration;

            string path = options.File.UseContentRoot
                              ? TitanLibHelper.BuildPath(contentRootPath, "logs", ".json")
                              : TitanLibHelper.BuildPath(options.File.Path, ".json");

            configuration.WriteTo.File(new JsonFormatter(), path,
                                       rollingInterval: TitanLibHelper.GetRollingInterval(options.File.RollingInterval),
                                       flushToDiskInterval: options.File.Period.ToTimeSpan(),
                                       rollOnFileSizeLimit: true, encoding: Encoding.UTF8);

            return configuration;
        }

        internal static LoggerConfiguration ConfigureSeq(this LoggerConfiguration configuration, TitanOptions options)
        {
            if (options.Seq == null || options.Seq.Enabled == false) return configuration;

            long batchPosting = options.Seq.RawIngestionPayload / options.Seq.EventBodySizeLimit;

            configuration.WriteTo.Seq(options.Seq.IngestionEndpoint,
                                      TitanLibHelper.GetLogLevel(options.Seq.LogLevelRestriction),
                                      period: options.Seq.Period.ToTimeSpan(), apiKey: options.Seq.Apikey, compact: true,
                                      eventBodyLimitBytes: options.Seq.EventBodySizeLimit, batchPostingLimit: (int) batchPosting,
                                      queueSizeLimit: options.Seq.QueueSizeLimit);

            return configuration;
        }

        internal static LoggerConfiguration ConfigureGrayLog(this LoggerConfiguration configuration, TitanOptions options)
        {
            var sinkOptions = new GraylogSinkOptions
            {
                Facility = options.GrayLog.Facility,
                Port = options.GrayLog.Port,
                TransportType = options.GrayLog.GetTransportType(),
                HostnameOrAddress = options.GrayLog.Address,
                StackTraceDepth = options.GrayLog.StackTraceDepth,
                MaxMessageSizeInUdp = options.GrayLog.MaxMessageSizeInUdp,
                ShortMessageMaxLength = options.GrayLog.ShortMessageMaxLength,
                MinimumLogEventLevel = options.GrayLog.GetMinimumLogEventLevel(),
                MessageGeneratorType = options.GrayLog.GetMessageIdGeneratorType()
            };
            return options.GrayLog == null || options.GrayLog.Enabled == false ? configuration : configuration.WriteTo.Graylog(sinkOptions);
        }
    }
}