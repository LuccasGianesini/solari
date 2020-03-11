using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Solari.Sol
{
    public interface ISolariMarshal
    {
        
        IApplicationBuilder ApplicationBuilder { get; }
        IHost Host { get; }
        IServiceProvider Provider { get; }
        /// <summary>
        /// Execute the stored build actions in sequence.
        /// </summary>
        /// <returns></returns>
        ISolariMarshal ExecuteBuildActions();
        /// <summary>
        /// Returns a service from the <see cref="IServiceProvider"/>.
        /// </summary>
        /// <typeparam name="TService">Type of the service</typeparam>
        /// <returns>TService Instance</returns>
        TService GetService<TService>();

        /// <summary>
        /// Executes a post configuration action based upon the <see cref="ISolariPostConfigure"/> interface.
        /// </summary>
        /// <param name="action">Action to be executed</param>
        /// <returns></returns>
        ISolariMarshal PostConfigureServices(Action<ISolariPostConfigure> action);
        // /// <summary>
        // /// Executes a post configuration action based upon the <see cref="IServiceProvider"/> interface.
        // /// </summary>
        // /// <param name="action">Action to be executed</param>
        // /// <returns></returns>
        // ISolariMarshal PostConfigureServices(Action<IServiceProvider> action);
        /// <summary>
        /// Set the <see cref="IApplicationBuilder"/>r property value.
        /// </summary>
        /// <param name="applicationBuilder"><see cref="IApplicationBuilder"/></param>
        /// <returns></returns>
        ISolariMarshal SetApplicationBuilder(IApplicationBuilder applicationBuilder);
        /// <summary>
        /// Set the <see cref="IHost"/> property value.
        /// </summary>
        /// <param name="host"><see cref="IHost"/></param>
        /// <returns></returns>
        ISolariMarshal SetHost(IHost host);

        /// <summary>
        /// Set the <see cref="IServiceProvider"/> property value.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        ISolariMarshal SetServiceProvider(IServiceProvider serviceProvider);
    }
}