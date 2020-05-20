using Solari.Deimos.Abstractions;

namespace Solari.Callisto.Tracer
{
    public static class TracingPluginManagerExtensions
    {
        public static ITracerPluginManager AddCallistoTracing(this ITracerPluginManager manager)
        {
            manager.Register(new CallistoTracerPlugin());
            return manager;
        }
    }
}