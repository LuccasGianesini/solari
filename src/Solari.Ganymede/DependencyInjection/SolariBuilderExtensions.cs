using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Solari.Ganymede.Domain;
using Solari.Ganymede.Domain.Options;
using Solari.Ganymede.Framework;
using Solari.Sol;
using Solari.Sol.Abstractions;

namespace Solari.Ganymede.DependencyInjection
{
    public static class SolariBuilderExtensions
    {
        public static ISolariBuilder AddGanymede(this ISolariBuilder builder,
                                                 Action<GanymedeClientActions> configureClients)
        {
            return builder.AddGanymede(null, configureClients);
        }

        public static ISolariBuilder AddGanymede(this ISolariBuilder builder,
                                                 Action<GanymedePolicyRegistry> configurePoliceRegistry,
                                                 Action<GanymedeClientActions> configureClients)
        {

            if (configurePoliceRegistry != null)
            {
                var registry = new GanymedePolicyRegistry();
                configurePoliceRegistry.Invoke(registry);
                builder.Services.AddPolicyRegistry(registry.PolicyRegistry);
            }

            configureClients(new GanymedeClientActions(builder, builder.Configuration));
            return builder;
        }
    }
}
