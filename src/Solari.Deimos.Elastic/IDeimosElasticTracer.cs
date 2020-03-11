using Elastic.Apm;
using Elastic.Apm.Api;

namespace Solari.Deimos.Elastic
{
    public interface IDeimosElasticTracer
    {
        ITracer Tracer { get; }
    }
}