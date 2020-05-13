using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Solari.Ganymede.Domain;
using Solari.Ganymede.Domain.Options;
using Solari.Ganymede.Framework;
using Solari.Sol;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Solari.Ganymede.DependencyInjection
{
    public static class SolariBuilderExtensions
    {
        public static ISolariBuilder AddGanymede(this ISolariBuilder builder,
                                                 Action<HttpClientActions> configureClients,
                                                 Action<PolicyActions> configurePoliceRegistry = null,
                                                 string yamlFile = GanymedeConstants.YamlFileName)
        {
            AddCoreServices(builder);
            configurePoliceRegistry?.Invoke(new PolicyActions(builder));
            configureClients(new HttpClientActions(builder, builder.AppConfiguration));

            return builder;
        }

        private static void AddCoreServices(ISolariBuilder builder)
        {
            builder.Services.Configure<GanymedePolicyOptions>(builder.AppConfiguration.GetSection(GanymedeConstants.HttpPolicies));
            builder
                .Services
                .AddSingleton<GanymedePolicyRegistry>()
                .AddPolicyRegistry();
            builder.Services.TryAddSingleton(provider => provider.GetRequiredService<GanymedePolicyRegistry>());
        }
    }
}