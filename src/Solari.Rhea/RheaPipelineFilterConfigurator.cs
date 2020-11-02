using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Solari.Sol;

namespace Solari.Rhea
{
    public class RheaPipelineFilterConfigurator
    {
        private readonly ISolariBuilder _builder;
        private readonly RheaPipeline _pipeline;

        public RheaPipelineFilterConfigurator(ISolariBuilder builder, RheaPipeline pipeline)
        {
            _builder = builder;
            _pipeline = pipeline;
        }

        public RheaPipelineFilterConfigurator AddFilter<T>() where T : class, IRheaPipelineFilter
        {
            _builder.Services.TryAddTransient<T>();
            _pipeline.AddFilter(typeof(T));
            return this;
        }

        public RheaPipelineFilterConfigurator AddFilter(IEnumerable<Type> filters)
        {
            foreach (Type filter in filters)
            {
                if (!filter.IsAssignableFrom(typeof(IRheaPipelineFilter)))
                {
                    // throw
                }

                _builder.Services.TryAddTransient(filter);
                _pipeline.AddFilter(filter);
            }

            return this;
        }
    }
}
