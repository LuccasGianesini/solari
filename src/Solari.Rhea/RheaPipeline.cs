using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Solari.Sol;
using Solari.Sol.Abstractions;

namespace Solari.Rhea
{
    public abstract class RheaPipeline
    {
        private readonly IServiceProvider _provider;
        private Type _first;
        public RheaPipeline(IServiceProvider provider)
        {
            _provider = provider;
        }

        public RheaPipeline AddFilter(Type filter)
        {
            if (!filter.IsAssignableFrom(typeof(IRheaPipelineFilter)))
            {
                // throw exception
            }

            if (_first != null)
                return this;
            _first = filter;
            return this;
        }

        public async Task Execute(PipelineContext context)
        {
            Check.ThrowIfNull(context, nameof(PipelineContext));

            var instance = _provider.GetRequiredService(_first) as IRheaPipelineFilter;
            Check.ThrowIfNull(instance, nameof(IRheaPipelineFilter));

            await instance.Call(context);
        }

    }
}
