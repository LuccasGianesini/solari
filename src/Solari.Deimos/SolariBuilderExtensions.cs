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
            var options = solariBuilder.AppConfiguration.GetOptions<DeimosOptions>(DeimosConstants.TracingAppSettingsSection);
            solariBuilder.Services.Configure<DeimosOptions>(solariBuilder.AppConfiguration.GetSection(DeimosConstants.TracingAppSettingsSection));
            solariBuilder.Services.AddSingleton<IDeimosTracer, DeimosTracer>();
            ConfigureTracing(solariBuilder, options);
            if (action == null) return solariBuilder;
            var manager = new TracerPluginManager(solariBuilder);
            action(manager);
            return solariBuilder;
        }

        private static void ConfigureTracing(ISolariBuilder solariBuilder, DeimosOptions options)
        {
            if (options.UseCorrelationId)
            {
                solariBuilder.AddDeimosCorrelationId(options.Http.UseMiddleware);
            }
            JaegerTracerConfiguration.AddJaeger(solariBuilder, options);
            
        }
    }
}