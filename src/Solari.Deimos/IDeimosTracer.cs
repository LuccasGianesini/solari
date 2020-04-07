using OpenTracing;

namespace Solari.Deimos
{
    public interface IDeimosTracer
    {
        ITracer JaegerTracer { get; }
        
        global::Elastic.Apm.Api.ITracer ElasticTracer { get; }
    }
}