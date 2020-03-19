using System;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using RawRabbit.Channel;
using RawRabbit.Common;
using RawRabbit.Configuration;
using RawRabbit.Enrichers.MessageContext;
using RawRabbit.Instantiation;
using RawRabbit.Operations;
using RawRabbit.vNext;
using Solari.Deimos.CorrelationId;
using Solari.Io;
using Solari.Miranda.Framework;
using Solari.Miranda.Options;
using Solari.Sol;

namespace Solari.Miranda.DependencyInjection
{
    public static class SolariBuilderExtensions
    {

        public static ISolariBuilder AddMiranda(this ISolariBuilder builder, Func<IRabbitMqPluginRegister, IRabbitMqPluginRegister> plugins = null)
        {
            return AddMiranda<BrokerCorrelationContext>(builder, plugins);
        }
        
        public static ISolariBuilder AddMiranda<TContext>(this ISolariBuilder builder, 
                                                          Func<IRabbitMqPluginRegister, IRabbitMqPluginRegister> plugins = null) 
            where TContext : class,new()
        {
            var options = builder.AppConfiguration.GetOptions<MirandaOptions>(MirandaLibConstants.AppSettingsSection);
            if(options == null)
                throw new InvalidOperationException("Unable to load Miranda options");
            var config = new RawRabbitConfiguration
            {
                Exchange = options.GetExchangeConfiguration(),
                Hostnames = options.Hostnames,
                Password = options.Password,
                Port = options.Port,
                Queue = options.GetQueueConfiguration(),
                Ssl = options.Ssl,
                Username = options.Username,
                AutomaticRecovery = options.AutomaticRecovery,
                GracefulShutdown = options.GetGracefulShutdownPeriod(),
                RecoveryInterval = options.GetRetryInterval(),
                RequestTimeout = options.GetRequestTimeout(),
                TopologyRecovery = options.TopologyRecovery,
                VirtualHost = options.VirtualHost,
                AutoCloseConnection = options.AutoCloseConnection,
                PersistentDeliveryMode = options.PersistentDeliveryMode,
                PublishConfirmTimeout = options.GetPublishConfirmTimeout(),
                RouteWithGlobalId = options.RouteWithGlobalId
            };
            ConfigureBus<TContext>(builder, plugins);
            return builder;
        }
        
        private static void ConfigureBus<TContext>(ISolariBuilder builder, Func<IRabbitMqPluginRegister, IRabbitMqPluginRegister> plugins = null)
            where TContext : class, new()
        {
            builder.Services.AddSingleton<IInstanceFactory>(serviceProvider =>
            {
                IRabbitMqPluginRegister register = plugins?.Invoke(new RabbitMqPluginRegister(serviceProvider));
                var options = serviceProvider.GetService<MirandaOptions>();
                var configuration = serviceProvider.GetService<RawRabbitConfiguration>();
                var namingConventions = new CustomNamingConventions(options.Namespace);

                return RawRabbitFactory.CreateInstanceFactory(new RawRabbitOptions
                {
                    DependencyInjection = ioc =>
                    {
                        register?.Register(ioc);
                        ioc.AddSingleton(options);
                        ioc.AddSingleton(configuration);
                        ioc.AddSingleton(serviceProvider);
                        ioc.AddSingleton<INamingConventions>(namingConventions);
                    },
                    Plugins = p =>
                    {
                        register?.Register(p);
                        p.UseAttributeRouting()
                         .UseRetryLater()
                         .UseMessageContext<TContext>()
                         .UseContextForwarding();

                        if (options.MessageProcessor?.Enabled == true)
                        {
                            p.Register(c => c.Use<ProcessUniqueMessagesMiddleware>());
                        }
                    }
                });
            });

            builder.Services.AddTransient(serviceProvider => serviceProvider.GetService<IInstanceFactory>().Create());
        }
    }
}