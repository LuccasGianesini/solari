using System;
using System.Text;
using Elastic.Apm.SerilogEnricher;
using Elastic.CommonSchema.Serilog;
using Microsoft.VisualBasic.CompilerServices;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.Graylog;
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

            configuration.WriteTo.Console();

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
                MinimumLogEventLevel = TitanLibHelper.GetLogLevel(options.LogLevelRestriction),
                AutoRegisterTemplate = options.Elk.AutoRegisterTemplate,
                AutoRegisterTemplateVersion = options.Elk.GetAutoRegisterTemplateVersion(),
                
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
            configuration.Enrich.WithElasticApmCorrelationInfo();

            return configuration;
        }
        internal static LoggerConfiguration ConfigureGreyLog(this LoggerConfiguration configuration, SerilogOptions options)
        {
            return !options.UseGreyLog || options.GreyLog == null ? configuration : configuration.WriteTo.Graylog(options.GreyLog);
        }
    }
}