using System;
using Elastic.Apm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTracing;
using Solari.Deimos.Abstractions;

namespace Solari.Deimos
{
    public class DeimosTracer : IDeimosTracer
    {
        public ITracer JaegerTracer { get; }
        public global::Elastic.Apm.Api.ITracer ElasticTracer { get; }

        // public DeimosTracer(IServiceProvider provider, IOptions<DeimosOptions> options)
        // {
        //     if (options.Value.UseElasticApm)
        //     {
        //         ElasticTracer = Agent.Tracer;
        //     }
        //
        //     if (options.Value.UseJaeger)
        //     {
        // -   JaegerTracer = provider.GetService<ITracer>();
        // }
    }
}