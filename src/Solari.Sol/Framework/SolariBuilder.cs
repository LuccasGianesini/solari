using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Solari.Sol.Extensions;

namespace Solari.Sol.Framework
{
    internal sealed class SolariBuilder : ISolariBuilder
    {
        private readonly ApplicationOptions _applicationOptions;

        public SolariBuilder(IServiceCollection services, IConfiguration configuration)
        {
            Services = services;
            Configuration = configuration;
            BuildActions = new Queue<BuildAction>(10);
            _applicationOptions = Configuration.GetApplicationOptions();
        }

        public IConfiguration Configuration { get; }
        public Queue<BuildAction> BuildActions { get; }

        public IServiceCollection Services { get; }

        public void AddBuildAction(BuildAction action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            BuildActions.Enqueue(action);
        }

        public ApplicationOptions GetAppOptions()
        {
            return _applicationOptions;
        }


        private IHostEnvironment GetHostEnvironment()
        {
            using (IServiceScope scope = Services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                return scope.ServiceProvider.GetService<IHostEnvironment>();
            }
        }
    }
}
