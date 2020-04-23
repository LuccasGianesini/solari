using System;
using System.Collections.Generic;
using System.Text;
using Elastic.CommonSchema.Serilog;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.Graylog;
using Serilog.Sinks.SystemConsole.Themes;
using Solari.Io;
using Solari.Sol;
using Solari.Titan.Abstractions;

namespace Solari.Titan.Framework
{
    internal static class SinksConfiguration
    {
        internal static LoggerConfiguration ConfigureConsole(this LoggerConfiguration configuration, bool useConsole)
        {
            if (!useConsole) return configuration;

            configuration.WriteTo
                         .Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss}][{Level:u3}]{Message:lj} {NewLine}{Exception}",theme: AnsiConsoleTheme.Code);

            return configuration;
        }

        internal static LoggerConfiguration ConfigureFile(this LoggerConfiguration configuration, SerilogOptions options, string contentRootPath = "")
        {
            if (!options.UseFile || options.File == null) return configuration;

            string path = options.File.UseContentRoot
                              ? TitanLibHelper.BuildPath(contentRootPath, "logs", ".json")
                              : TitanLibHelper.BuildPath(options.File.Path, ".json");

            configuration.WriteTo.File(new JsonFormatter(), path,
                                       rollingInterval: TitanLibHelper.GetRollingInterval(options.File.RollingInterval),
                                       flushToDiskInterval: options.File.Period.ToTimeSpan(),
                                       rollOnFileSizeLimit: true, encoding: Encoding.UTF8);

            return configuration;
        }

        internal static LoggerConfiguration ConfigureSeq(this LoggerConfiguration configuration, SerilogOptions options)
        {
            if (!options.UseSeq || options.Seq == null) return configuration;

            long batchPosting = options.Seq.RawIngestionPayload / options.Seq.EventBodySizeLimit;

            configuration.WriteTo.Seq(options.Seq.IngestionEndpoint,
                                      TitanLibHelper.GetLogLevel(options.LogLevelRestriction),
                                      period: options.Seq.Period.ToTimeSpan(), apiKey: options.Seq.Apikey, compact: true,
                                      eventBodyLimitBytes: options.Seq.EventBodySizeLimit, batchPostingLimit: (int) batchPosting,
                                      queueSizeLimit: options.Seq.QueueSizeLimit);

            return configuration;
        }

        internal static LoggerConfiguration ConfigureElasticSearch(this LoggerConfiguration configuration, SerilogOptions options, ApplicationOptions applicationOptions)
        {
            if (!options.UseElk || options.Elk == null) return configuration;
            var elastic = new ElasticsearchSinkOptions(new Uri(options.Elk.Url))
            {
                EmitEventFailure = EmitEventFailureHandling.RaiseCallback,
                FailureCallback = e =>
                {
                    StringBuilder message = new StringBuilder()
                                            .Append("Error submitting events to elasticsearch sink: ")
                                            .Append("Template: ").Append(e.MessageTemplate).AppendLine()
                                            .Append("Timestamp: ").Append(e.Timestamp).AppendLine()
                                            .Append("Level: ").Append(e.Level).AppendLine()
                                            .Append("Exception: ").Append(e.Exception).AppendLine()
                                            .Append("Properties: ").AppendLine();


                    foreach (KeyValuePair<string,LogEventPropertyValue> keyValuePair in e.Properties)
                    {
                        message.Append(keyValuePair.Key).Append(":").Append(keyValuePair.Value).AppendLine();
                    }
                        
                    Log.Error(message.ToString());
                },
                
                MinimumLogEventLevel = TitanLibHelper.GetLogLevel(options.LogLevelRestriction),
                AutoRegisterTemplate = options.Elk.AutoRegisterTemplate,
                AutoRegisterTemplateVersion = options.Elk.GetAutoRegisterTemplateVersion(),
                BufferFileCountLimit = options.Elk.BufferFileCountLimit,
                Period = options.Elk.GetPeriod(),
                BatchPostingLimit = options.Elk.BatchPostingLimit,
                QueueSizeLimit = options.Elk.QueueSizeLimit,
                BufferCleanPayload = (failingEvent, statuscode, exception) =>
                {
                    Log.Error($"Error while sending payload to server. Code: {statuscode}  Message: {exception}");
                    return exception;
                },
                BufferLogShippingInterval = options.Elk.GetBufferLogShippingInterval(),
                IndexFormat = string.IsNullOrWhiteSpace(options.Elk.IndexFormat)
                                  ? $"{applicationOptions.ApplicationName}-{DateTime.Now:dd-MM-yyyy}"
                                  : options.Elk.IndexFormat,
                ModifyConnectionSettings = connectionConfiguration =>
                    options.Elk.BasicAuthEnabled
                        ? connectionConfiguration.BasicAuthentication(options.Elk.Username, options.Elk.Password)
                        : connectionConfiguration,
                CustomFormatter = new EcsTextFormatter()
            };
            configuration.WriteTo.Elasticsearch(elastic);

            return configuration;
        }
        internal static LoggerConfiguration ConfigureGreyLog(this LoggerConfiguration configuration, SerilogOptions options)
        {
            return !options.UseGreyLog || options.GreyLog == null ? configuration : configuration.WriteTo.Graylog(options.GreyLog);
        }
    }
}