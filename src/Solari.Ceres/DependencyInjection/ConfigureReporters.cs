using System;
using App.Metrics;
using App.Metrics.Formatters.InfluxDB;
using Solari.Ceres.Abstractions;

namespace Solari.Ceres.DependencyInjection
{
    public class ConfigureReporters
    {
        public static IMetricsBuilder ConfigureGraphiteReporter(CeresOptions options, IMetricsBuilder builder)
        {
            return builder.Report.ToInfluxDb(build =>
            {
                build.InfluxDb.BaseUri = new Uri(options.InfluxDb.Uri);
                build.InfluxDb.Consistenency = options.InfluxDb.Consistency;
                build.InfluxDb.Database = options.InfluxDb.Database;
                build.InfluxDb.Password = options.InfluxDb.Password;
                build.InfluxDb.UserName = options.InfluxDb.UserName;
                build.InfluxDb.RetentionPolicy = options.InfluxDb.RetentionPolicy;
                build.InfluxDb.CreateDataBaseIfNotExists = options.InfluxDb.CreateDataBaseIfNotExists;
                build.FlushInterval = options.InfluxDb.GetFlushInterval();
                build.HttpPolicy.Timeout = options.InfluxDb.GetTimeout();
                build.HttpPolicy.BackoffPeriod = options.InfluxDb.GetBackoffPeriod();
                build.HttpPolicy.FailuresBeforeBackoff = options.InfluxDb.FailuresBeforeBackoff;
                build.MetricsOutputFormatter = new MetricsInfluxDbLineProtocolOutputFormatter();
            });
        }

        public static IMetricsBuilder ConfigurePrometheus(CeresOptions options, IMetricsBuilder builder)
        {
            if (options.Prometheus == null)
            {
                builder.OutputMetrics.AsPrometheusProtobuf();
                builder.OutputMetrics.AsPrometheusPlainText();
                return builder;
            }

            switch (options.Prometheus.OutputFormat)
            {
                case "proto":
                    builder.OutputMetrics.AsPrometheusProtobuf();
                    break;
                case "text":
                    builder.OutputMetrics.AsPrometheusPlainText();
                    break;
                default:
                    builder.OutputMetrics.AsPrometheusProtobuf();
                    builder.OutputMetrics.AsPrometheusPlainText();
                    break;
            }

            return builder;
        }
    }
}