using System;
using System.IO;
using System.Linq;
using HealthChecks.UI.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solari.Io.Abstractions;
using Solari.Sol;
using Solari.Sol.Extensions;
using Solari.Titan.Abstractions;
using SeqOptions = Microsoft.Extensions.DependencyInjection.SeqOptions;

namespace Solari.Io
{
    public static class SolariBuilderExtensions
    {
        public static ISolariBuilder AddIo(this ISolariBuilder builder, Action<IHealthChecksBuilder> addChecks = null)
        {
            IConfigurationSection section = builder.AppConfiguration.GetSection(IoConstants.AppSettingsSection);
            if (!section.Exists())
                throw new IOException("Io AppSettings section not found!");
            var options = builder.AppConfiguration.GetOptions<IoOptions>(section);
            builder.Services.Configure<IoOptions>(section);
            if (!options.Enabled)
                return builder;
            ConfigureAspNetHealthCheckInfrastructure(builder, options, addChecks);
            ConfigureUi(builder, options);


            return builder;
        }

        private static void ConfigureAspNetHealthCheckInfrastructure(ISolariBuilder builder, IoOptions options,
                                                                     Action<IHealthChecksBuilder> addChecks)
        {
            IHealthChecksBuilder healthChecks = builder.Services.AddHealthChecks();
            addChecks?.Invoke(healthChecks);
            ConfigureSeqPublisher(builder, options, healthChecks);
            ConfigurePrometheusGatewayPublisher(builder, options, healthChecks);
        }

        private static void ConfigureUi(ISolariBuilder builder, IoOptions options)
        {
            if (!options.EnableUi) return;

            builder.Services.AddHealthChecksUI(a =>
                   {
                       a.DisableDatabaseMigrations();
                       a.SetEvaluationTimeInSeconds(options.Interval);
                       ConfigureHealthEndpoints(builder, a, options);
                       ConfigureWebHooks(a, options);
                   })
                   .AddInMemoryStorage();
            builder.AddBuildAction(new BuildAction("HealthCheckUI")
            {
                Action = provider =>
                {
                    var app = provider.GetService<IApplicationBuilder>();
                    if (app == null)
                        return;
                    app.UseHealthChecksUI();
                }
            });
        }

        public static void ConfigureWebHooks(Settings settings, IoOptions options)
        {
            if (options.WebHooks == null || !options.WebHooks.Any()) return;

            options.WebHooks.ForEach(a => settings.AddWebhookNotification(a.Name, a.Uri, a.Payload, a.RestoredPayload));
        }


        public static void ConfigureHealthEndpoints(ISolariBuilder builder, Settings settings, IoOptions options)
        {
            if (options.Endpoints == null || !options.Endpoints.Any())
            {
                settings.AddHealthCheckEndpoint(builder.GetAppOptions().ApplicationName, options.HealthEndpoint);
                return;
            }

            options.Endpoints.ForEach(a => settings.AddHealthCheckEndpoint(a.Name, a.Uri));
        }

        private static void ConfigurePrometheusGatewayPublisher(ISolariBuilder builder, IoOptions options, IHealthChecksBuilder healthChecksBuilder)
        {
            if (options.PrometheusGateway is null)
                return;
            if (!options.PrometheusGateway.Enabled)
                return;
            if (string.IsNullOrEmpty(options.PrometheusGateway.Address))
                throw new SolariIoException("Prometheus push gateway address is null or empty.");
            ApplicationOptions appOptions = builder.GetAppOptions();
            healthChecksBuilder.AddPrometheusGatewayPublisher(options.PrometheusGateway.Address, appOptions.ApplicationName,
                                                              options.PrometheusGateway.Instance);
        }

        private static void ConfigureSeqPublisher(ISolariBuilder builder, IoOptions options, IHealthChecksBuilder healthChecksBuilder)
        {
            if (options.Seq is null)
                return;
            if (options.Seq.Enabled == false)
                return;
            if (options.Seq.UseTitanConfiguration)
            {
                ConfigureSeqPublisherFromTitanOptions(builder, healthChecksBuilder);
                return;
            }

            healthChecksBuilder.AddSeqPublisher(CreateSeqPublisherOptions(options.Seq.IngestionEndpoint, options.Seq.ApiKey));
        }

        private static void ConfigureSeqPublisherFromTitanOptions(ISolariBuilder builder, IHealthChecksBuilder healthChecksBuilder)
        {
            IConfigurationSection section = builder.AppConfiguration.GetSection(TitanConstants.TitanAppSettingsSection);
            if (!section.Exists())
                throw new SolariIoException("Unable to get Titan configuration section!");
            var titanOptions = builder.AppConfiguration.GetOptions<TitanOptions>(section);

            healthChecksBuilder.AddSeqPublisher(CreateSeqPublisherOptions(titanOptions.Seq.IngestionEndpoint, titanOptions.Seq.Apikey));
        }

        private static Action<SeqOptions> CreateSeqPublisherOptions(string endpoint, string apiKey)
        {
            if (string.IsNullOrEmpty(endpoint))
                throw new SolariIoException("Seq ingestion endpoint is null or empty.");
            return options =>
            {
                options.Endpoint = endpoint;
                options.ApiKey = apiKey;
            };
        }
    }
}