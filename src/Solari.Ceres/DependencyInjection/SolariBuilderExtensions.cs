using App.Metrics;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solari.Ceres.Abstractions;
using Solari.Ceres.Framework;
using Solari.Sol;
using Solari.Sol.Extensions;

namespace Solari.Ceres.DependencyInjection
{
    public static class SolariBuilderExtensions
    {
        public static ISolariBuilder AddCeres(this ISolariBuilder builder)
        {
            IConfigurationSection section = builder.AppConfiguration.GetSection(CeresConstants.AppSettingsSection);
            if (!section.Exists())
                throw new CeresException("Ceres AppSettings section not found!");

            var options = builder.AppConfiguration.GetOptions<CeresOptions>(section);

            ApplicationOptions appOptions = builder.GetAppOptions();
            var metricsBuilder = new MetricsBuilder();
            if (!options.Enabled) return builder;

            builder.Services.Configure<CeresOptions>(builder.AppConfiguration.GetSection(CeresConstants.AppSettingsSection));

            ConfigureMetricsBuilder(metricsBuilder, options, appOptions);
            builder.Services.AddMetrics(metricsBuilder.Build());
            builder.Services.AddMetricsEndpoints();
            ConfigureReporters.ConfigurePrometheus(options, metricsBuilder);
            ConfigureInfluxDb(builder, options, metricsBuilder);
            ConfigureEndpoints(builder, options);
            ConfigureMiddleware(builder, options.Middlewares);
            ConfigureCpuUsageMetric(builder, options.CollectCpuMetrics);
            ConfigureMemoryUsageMetric(builder, options.CollectMemoryMetrics);
            return builder;
        }

        private static void ConfigureInfluxDb(ISolariBuilder builder, CeresOptions options, IMetricsBuilder metricsBuilder)
        {
            if (options.InfluxDb == null || !options.InfluxDb.Enabled)
                return;
            ConfigureReporters.ConfigureInfluxDbReporter(options, metricsBuilder);
            builder.Services.AddMetricsReportingHostedService();
        }

        private static void ConfigureCpuUsageMetric(ISolariBuilder builder, bool enabled)
        {
            if (!enabled)
                return;
            builder.Services.AddHostedService<CpuMeasurementHostedService>();
        }

        private static void ConfigureMemoryUsageMetric(ISolariBuilder builder, bool enabled)
        {
            if (!enabled)
                return;
            builder.Services.AddHostedService<MemoryMeasurementHostedService>();
        }


        private static void ConfigureMetricsBuilder(IMetricsBuilder metricsBuilder, CeresOptions options, ApplicationOptions appOptions)
        {
            metricsBuilder.Configuration.Configure(a =>
            {
                a.Enabled = options.Enabled;
                a.AddServerTag();
                a.AddEnvTag();
                a.GlobalTags.Add("app", appOptions.ApplicationName);
                a.DefaultContextLabel = appOptions.ApplicationName;
                a.ReportingEnabled = options.InfluxDb.Enabled;
            });
        }

        private static void ConfigureEndpoints(ISolariBuilder builder, CeresOptions options)
        {
            builder.AddBuildAction(new BuildAction("Ceres Endpoints")
            {
                Action = provider =>
                {
                    var appBuilder = provider.GetService<ISolariMarshal>();
                    if (appBuilder == null) return;

                    if (options.UseEnvEndpoint)
                        appBuilder.ApplicationBuilder.UseEnvInfoEndpoint();
                    if (options.UseProtoEndpoint)
                        appBuilder.ApplicationBuilder.UseMetricsEndpoint();
                    if (options.UseTextEndpoint)
                        appBuilder.ApplicationBuilder.UseMetricsTextEndpoint();
                }
            });
        }

        private static void ConfigureMiddleware(ISolariBuilder builder, MetricsTrackingMiddlewareOptions options)
        {
            if (options is null)
                return;

            builder.Services.AddMetricsTrackingMiddleware(a =>
            {
                a.ApdexTrackingEnabled = options.ApdexTracking;
                a.ApdexTSeconds = options.ApdexSeconds;
                a.IgnoredHttpStatusCodes = options.IgnoredHttpStatusCodes;
                a.OAuth2TrackingEnabled = options.OAuth2Tracking;
            });

            builder.AddBuildAction(new BuildAction("Ceres Middleware")
            {
                Action = provider =>
                {
                    var appBuilder = provider.GetService<ISolariMarshal>();
                    if (appBuilder == null) return;

                    if (options.ApdexTracking)
                        appBuilder.ApplicationBuilder.UseMetricsApdexTrackingMiddleware();
                    if (options.PostAndPutSizeTracking)
                        appBuilder.ApplicationBuilder.UseMetricsPostAndPutSizeTrackingMiddleware();
                    if (options.RequestTracking)
                        appBuilder.ApplicationBuilder.UseMetricsRequestTrackingMiddleware();
                    if (options.OAuth2Tracking)
                        appBuilder.ApplicationBuilder.UseMetricsOAuth2TrackingMiddleware();
                    if (options.ErrorTracking)
                        appBuilder.ApplicationBuilder.UseMetricsErrorTrackingMiddleware();
                    if (options.ActiveRequests)
                        appBuilder.ApplicationBuilder.UseMetricsActiveRequestMiddleware();
                }
            });
        }
    }
}