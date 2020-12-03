using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Solari.Ceres.DependencyInjection;
using Solari.Deimos;
using Solari.Deimos.Abstractions;
using Solari.Io;
using Solari.Sol;
using Solari.Sol.Abstractions;

namespace Solari.Themis
{
    public static class SolariBuilderExtensions
    {
        public static ISolariBuilder AddThemis(this ISolariBuilder builder, bool addHealthChecks,
                                               Action<ITracerPluginManager> tracingPlugins = null,
                                               Action<IHealthChecksBuilder> healthChecks = null)
        {
            if(addHealthChecks)
                builder.AddIo(healthChecks);

            builder.AddDeimos(tracingPlugins);
            builder.Services.TryAdd(ServiceDescriptor.Singleton(typeof(IThemis<>), typeof(Themis<>)));
            return builder;
        }
    }
}
