using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Solari.Sol;

namespace Solari.Rhea
{
    public class RheaPipelineConfigurator
    {
        private readonly ISolariBuilder _builder;

        public RheaPipelineConfigurator(ISolariBuilder builder)
        {
            _builder = builder;
        }


        public RheaPipelineConfigurator RegisterPipeline<T>(Action<RheaPipelineFilterConfigurator> configureFilters) where T : RheaPipeline
        {
            _builder.Services.Add(ServiceDescriptor.Describe(typeof(T), provider =>
            {
                Check.ThrowIfNull(configureFilters, nameof(Action<RheaPipelineFilterConfigurator>));
                var instance = ActivatorUtilities.GetServiceOrCreateInstance(provider, typeof(T)) as RheaPipeline;
                configureFilters.Invoke(new RheaPipelineFilterConfigurator(_builder, instance));
                return instance;
            }, ServiceLifetime.Transient));
            return this;
        }



    }
}
