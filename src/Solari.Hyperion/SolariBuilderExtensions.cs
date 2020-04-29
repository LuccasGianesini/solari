using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solari.Hyperion.Abstractions;
using Solari.Io;
using Solari.Sol;

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
            ConfigureClient(builder, options);
            RegisterService(builder, options);
            return builder;
        }

        private static void ConfigureClient(ISolariBuilder builder, HyperionOptions options)
        {
            builder.Services.AddSingleton<IConsulClient>(provider => new ConsulClient(a =>
            {
                a.Address = new Uri(options.ConsulAddress);
                a.Token = options.ConsulToken;
                a.Datacenter = options.Datacenter;
            }));
        }

        private static void RegisterService(ISolariBuilder builder, HyperionOptions hyperionOptions)
        {
            if (!hyperionOptions.Register)
                return;
            builder.Services.AddHostedService<HyperionStartupProcedure>();
        }
    }
}