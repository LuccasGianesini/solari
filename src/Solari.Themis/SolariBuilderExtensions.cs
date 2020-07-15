using System;
using Microsoft.Extensions.DependencyInjection;
using Solari.Ceres.DependencyInjection;
using Solari.Deimos;
using Solari.Deimos.Abstractions;
using Solari.Io;
using Solari.Sol;

namespace Solari.Themis
{
    public static class SolariBuilderExtensions
    {
        public static ISolariBuilder AddThemis(this ISolariBuilder builder, Action<ITracerPluginManager> tracingPlugins = null,
                                               Action<IHealthChecksBuilder> healthChecks = null)
        {
            builder.AddIo(healthChecks);
            // builder.AddCeres();
            builder.AddDeimos(tracingPlugins);
            builder.Services.AddTransient<IThemis, Themis>();
            return builder;
        }

        public static ISolariBuilder AddThemisWithDeimosOnly(this ISolariBuilder builder, Action<ITracerPluginManager>
                                                                 tracingPlugins = null)
        {
            builder.Services.AddTransient<IThemis, ThemisNoMetrics>();
            builder.AddDeimos(tracingPlugins);
            return builder;
        }
    }
}
