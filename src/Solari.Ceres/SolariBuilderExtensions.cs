using System;
using App.Metrics;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Solari.Ceres.Abstractions;
using Solari.Io;
using Solari.Sol;

namespace Solari.Ceres
{
    public static class SolariBuilderExtensions
    {
        public static ISolariBuilder AddCeres(this ISolariBuilder builder)
        {
            var options = builder.AppConfiguration.GetOptions<CeresOptions>(CeresConstants.AppSettingsSection);
            var appOptions = builder.GetAppOptions();
            var metricsBuilder = new MetricsBuilder();

            options ??= new CeresOptions();
            if (!options.Enabled)
            {
                return builder;
            }

            ConfigureMetricsBuilder(metricsBuilder, options, appOptions);
            builder.Services.AddMetrics(metricsBuilder.Build());
            builder.Services.AddMetricsEndpoints();

            ConfigureReporters.ConfigurePrometheus(options, metricsBuilder);
            if (options.InfluxDb.Enabled)
            {
                ConfigureReporters.ConfigureGraphiteReporter(options, metricsBuilder);
                builder.Services.AddMetricsReportingHostedService();
            }
            ConfigureEndpoints(builder, options);
            ConfigureMiddleware(builder, options);
            
            return builder;
        }

        private static void ConfigureMetricsBuilder(MetricsBuilder metricsBuilder, CeresOptions options, ApplicationOptions appOptions)
        {
            metricsBuilder.Configuration.Configure(a =>
            {
                a.Enabled = options.Enabled;
                a.AddServerTag();
                a.AddEnvTag();
                a.GlobalTags.Add("app", appOptions.ApplicationName.ToLowerInvariant().Underscore());
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
                    if (appBuilder == null)
                    {
                        return;
                    }

                    if (options.UseEnvEndpoint)
                        appBuilder.ApplicationBuilder.UseEnvInfoEndpoint();
                    if (options.UseProtoEndpoint)
                        appBuilder.ApplicationBuilder.UseMetricsEndpoint();
                    if (options.UseTextEndpoint)
                        appBuilder.ApplicationBuilder.UseMetricsTextEndpoint();
                }
            });
        }

        private static void ConfigureMiddleware(ISolariBuilder builder, CeresOptions options)
        {
            builder.Services.AddMetricsTrackingMiddleware(a =>
            {
                a.ApdexTrackingEnabled = options.Middlewares.ApdexTracking;
                a.ApdexTSeconds = options.Middlewares.ApdexSeconds;
                a.IgnoredHttpStatusCodes = options.Middlewares.IgnoredHttpStatusCodes;
                a.OAuth2TrackingEnabled = options.Middlewares.OAuth2Tracking;
            });

            builder.AddBuildAction(new BuildAction("Ceres Middleware")
            {
                Action = provider =>
                {
                    var appBuilder = provider.GetService<ISolariMarshal>();
                    if (appBuilder == null)
                    {
                        return;
                    }

                    if (options.Middlewares.ApdexTracking)
                        appBuilder.ApplicationBuilder.UseMetricsApdexTrackingMiddleware();
                    if (options.Middlewares.PostAndPutSizeTracking)
                        appBuilder.ApplicationBuilder.UseMetricsPostAndPutSizeTrackingMiddleware();
                    if (options.Middlewares.RequestTracking)
                        appBuilder.ApplicationBuilder.UseMetricsRequestTrackingMiddleware();
                    if (options.Middlewares.OAuth2Tracking)
                        appBuilder.ApplicationBuilder.UseMetricsOAuth2TrackingMiddleware();
                    if (options.Middlewares.UseErrorTracking)
                        appBuilder.ApplicationBuilder.UseMetricsErrorTrackingMiddleware();
                    if (options.Middlewares.UseActiveRequests)
                        appBuilder.ApplicationBuilder.UseMetricsActiveRequestMiddleware();
                    if (options.Middlewares.RequestTracking)
                        appBuilder.ApplicationBuilder.UseMetricsRequestTrackingMiddleware();
                }
            });
        }
    }
}