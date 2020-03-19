using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Solari.Sol.Framework
{
    internal sealed class SolariBuilder : ISolariBuilder
    {
        public SolariBuilder(IServiceCollection services, IConfiguration configuration)
        {
            Services = services;
            AppConfiguration = configuration;
            BuildActions = new Queue<BuildAction>(5);
            HostEnvironment = GetHostEnvironment();
            ApplicationAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
        }


        public List<Assembly> ApplicationAssemblies { get; }
        public IConfiguration AppConfiguration { get; }
        public Queue<BuildAction> BuildActions { get; }

        public IServiceCollection Services { get; }

        public void AddBuildAction(BuildAction action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            BuildActions.Enqueue(action);
        }

        /// <summary>
        /// Get the <see cref="IHostEnvironment"/>. This property requires the <see cref="IServiceProvider"/> to be built.
        /// </summary>
        public IHostEnvironment HostEnvironment { get; }

        private IHostEnvironment GetHostEnvironment()
        {
            using (IServiceScope scope = Services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                return scope.ServiceProvider.GetService<IHostEnvironment>();
            }
        }
    }
}