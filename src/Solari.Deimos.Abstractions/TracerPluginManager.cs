using System;
using Solari.Sol;

namespace Solari.Deimos.Abstractions
{
    public class TracerPluginManager : ITracerPluginManager
    {
        private readonly ISolariBuilder _builder;

        public TracerPluginManager(ISolariBuilder builder) { _builder = builder; }

        public ITracerPluginManager Register(ITracerPlugin plugin)
        {
            if (plugin == null) throw new ArgumentNullException(nameof(plugin));
            plugin.Builder = _builder;
            plugin.Configure();
            return this;
        }
    }
}