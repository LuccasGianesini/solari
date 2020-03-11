using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Solari.Sol
{
    public interface ISolariBuilder
    {
        /// <summary>
        /// Application configuration.
        /// </summary>
        IConfiguration AppConfiguration { get; }
        /// <summary>
        /// Stored build actions to be executed when UseSol is called.
        /// </summary>
        Queue<Action<IServiceProvider>> BuildActions { get; }
        
        /// <summary>
        /// Dotnet core DI container.
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// Adds a build action into the build action queue.
        /// </summary>
        /// <param name="action"></param>
        void AddBuildAction(Action<IServiceProvider> action);

        /// <summary>
        /// Application host environment.
        /// </summary>
        IHostEnvironment HostEnvironment { get; }
    }
}