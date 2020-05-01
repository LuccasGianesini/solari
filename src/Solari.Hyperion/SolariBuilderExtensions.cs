using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solari.Hyperion.Abstractions;
using Solari.Sol;
using Solari.Sol.Extensions;

namespace Solari.Hyperion
{
    public static class SolariBuilderExtensions
    {
        public static ISolariBuilder AddHyperion(this ISolariBuilder builder)
        {
            if (!builder.AppConfiguration.GetSection(HyperionConstants.AppSettingsSection).Exists())
                return builder;
            var options = builder.AppConfiguration.GetOptions<HyperionOptions>(HyperionConstants.AppSettingsSection);
            builder.Services.Configure<HyperionOptions>(builder.AppConfiguration.GetSection(HyperionConstants.AppSettingsSection));
            builder.Services.AddSingleton<HyperionStartupProcedure, HyperionStartupProcedure>();
            builder.Services.AddSingleton<IHyperionClient, HyperionClient>();
            builder.Services.AddSingleton<IKvOperations, KvOperations>();
            builder.Services.AddSingleton<IServiceOperations, ServiceOperations>();
            builder.Services.AddSingleton<IConsulClientFactory, ConsulClientFactory>();
            builder.Services.AddSingleton(provider => provider.GetService<IConsulClientFactory>().Create(options));
            RegisterService(builder, options);
            return builder;
        }

        private static void RegisterService(ISolariBuilder builder, HyperionOptions hyperionOptions)
        {
            if (!hyperionOptions.Register)
                return;
            builder.Services.AddHostedService<HyperionStartupProcedure>();
        }
    }
}