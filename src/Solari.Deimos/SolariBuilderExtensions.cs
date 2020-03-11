using System;
using Microsoft.Extensions.DependencyInjection;
using Solari.Deimos.Abstractions;
using Solari.Deimos.CorrelationId;
using Solari.Io;
using Solari.Sol;

namespace Solari.Deimos
{
    public static class SolariBuilderExtensions
    {
        public static ISolariBuilder AddDeimos(this ISolariBuilder solariBuilder, Action<ITracerPluginManager> action = null)
        {
            if (action != null)
            {
                var manager = new TracerPluginManager(solariBuilder);
                action(manager);
            }
            
            var options = solariBuilder.AppConfiguration.GetOptions<DeimosOptions>(DeimosConstants.TracingAppSettingsSection);
            solariBuilder.Services.Configure<DeimosOptions>(solariBuilder.AppConfiguration.GetSection(DeimosConstants.TracingAppSettingsSection));
            ConfigureTracing(solariBuilder, options);

            return solariBuilder;
        }

        private static void ConfigureTracing(ISolariBuilder solariBuilder, DeimosOptions options)
        {
            if (options.UseCorrelationId)
            {
                solariBuilder.AddDeimosCorrelationId(options.Http.UseMiddleware);
            }

            if (options.UseJaeger && !options.UseElasticApm)
            {
                Solari.Deimos.Jaeger.JaegerTracerConfiguration.AddDeimosJaeger(solariBuilder, options);
            }

            if (options.UseElasticApm && !options.UseJaeger)
            {
                Solari.Deimos.Elastic.ElasticTracerConfiguration.AddElasticApm(solariBuilder, options);
            }
        }
    }
}