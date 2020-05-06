namespace Solari.Deimos.Abstractions
{
    public interface ITracerPluginManager
    {
        /// <summary>
        ///     Register a new tracer plugin and calls the <see cref="ITracerPlugin.Configure" />
        /// </summary>
        /// <param name="plugin">The plugin</param>
        /// <returns></returns>
        ITracerPluginManager Register(ITracerPlugin plugin);
    }
}