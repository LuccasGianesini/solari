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

        /// <summary>
        /// Add the entrypoint to the pipeline.
        /// This method can only be called once
        /// </summary>
        /// <param name="filter">Filter type</param>
        /// <returns><see cref="RheaPipeline"/></returns>
        public RheaPipeline AddFirstFilter(Type filter)
        {
            if (!filter.IsAssignableFrom(typeof(IRheaPipelineFilter)))
            {
                // throw exception
            }

            if (_first is not null)
                throw new SolariException("First filter is already registered");
            _first = filter;
            return this;
        }

        /// <summary>
        /// Executes the pipeline calling the first filter.
        /// </summary>
        /// <param name="context">Pipeline context. <see cref="PipelineContext"/></param>
        /// <returns><see cref="Task"/></returns>
        public async Task Execute(PipelineContext context)
        {
            Check.ThrowIfNull(context, nameof(PipelineContext));

            var instance = _provider.GetRequiredService(_first) as IRheaPipelineFilter;
            Check.ThrowIfNull(instance, nameof(IRheaPipelineFilter));

            await instance.Call(context);
        }

    }
}
