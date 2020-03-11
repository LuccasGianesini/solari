using System;
using Solari.Sol;

namespace Solari.Deimos.Abstractions
{
    public interface ITracerPlugin
    {
        /// <summary>
        /// <see cref="ISolariBuilder"/>. This property is set by <see cref="ITracerPluginManager"/>
        /// </summary>
        ISolariBuilder Builder { get; set; }
        /// <summary>
        ///  Configures the plugin using <see cref="ISolariBuilder"/> resources.
        ///  <remarks>
        ///     In case both methods are implemented, this method is not going to be invoked.
        /// </remarks>
        /// </summary>
        void Configure();

    }
}