using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solari.Deimos.Abstractions;
using Solari.Deimos.CorrelationId;
using Solari.Sol;
using Solari.Sol.Abstractions;
using Solari.Sol.Abstractions.Extensions;

namespace Solari.Deimos
{
    public static class SolariBuilderExtensions
    {
        public static ISolariBuilder AddDeimos(this ISolariBuilder solariBuilder, Action<ITracerPluginManager> plugins = null)
        {
            IConfigurationSection section = solariBuilder.Configuration.GetSection(DeimosConstants.TracingAppSettingsSection);
            if (!section.Exists())
                throw new DeimosException("Deimos AppSettings section not found!");

            var options = section.GetOptions<DeimosOptions>();
            solariBuilder.Services.Configure<DeimosOptions>(section);

            ConfigureTracing(solariBuilder, options);

            if (plugins == null) return solariBuilder;
            var manager = new TracerPluginManager(solariBuilder);
            plugins(manager);
            return solariBuilder;
        }

        private static void ConfigureTracing(ISolariBuilder solariBuilder, DeimosOptions options)
        {
            solariBuilder.AddDeimosCorrelationId(options.Http.UseMiddleware);
            JaegerTracerConfiguration.AddJaeger(solariBuilder, options);
        }
    }
}
