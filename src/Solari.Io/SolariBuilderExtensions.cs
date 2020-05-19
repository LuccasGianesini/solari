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
        
        

        
    }
}