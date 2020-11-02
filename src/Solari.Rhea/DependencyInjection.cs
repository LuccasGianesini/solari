using System;
using Solari.Sol;
using Solari.Vanth.DependencyInjection;

namespace Solari.Rhea
{
    public static class DependencyInjection
    {
        public static ISolariBuilder AddRhea(this ISolariBuilder builder, Action<RheaPipelineConfigurator> configure)
        {
            Check.ThrowIfNull(configure, nameof(Action<RheaPipelineConfigurator>));
            builder.AddVanth();
            configure(new RheaPipelineConfigurator(builder));
            return builder;
        }

    }
}
