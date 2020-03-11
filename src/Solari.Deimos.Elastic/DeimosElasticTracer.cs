using System.Collections.Concurrent;
using Elastic.Apm;
using Elastic.Apm.Api;

namespace Solari.Deimos.Elastic
{
    public class DeimosElasticTracer : IDeimosElasticTracer
    {
        public ITracer Tracer { get; }

        public DeimosElasticTracer(ITracer tracer)
        {
            Tracer = tracer;
        }
        
        
    }
}