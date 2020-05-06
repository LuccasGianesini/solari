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
        ///     Execute the stored build actions in sequence.
        /// </summary>
        /// <returns></returns>
        ISolariMarshal ExecuteBuildActions();

        ISolariMarshal ExecuteBuildAsyncActions();

        /// <summary>
        ///     Returns a service from the <see cref="IServiceProvider" />.
        /// </summary>
        /// <typeparam name="TService">Type of the service</typeparam>
        /// <returns>TService Instance</returns>
        TService GetService<TService>();

        /// <summary>
        ///     Executes a post configuration action based upon the <see cref="ISolariPostConfigure" /> interface.
        /// </summary>
        /// <param name="action">Action to be executed</param>
        /// <returns></returns>
        ISolariMarshal PostConfigureServices(Action<ISolariPostConfigure> action);

        ISolariMarshal ConfigureApplication(IServiceProvider provider, IApplicationBuilder applicationBuilder, IHost host);
    }
}