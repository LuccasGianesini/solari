using MassTransit;
using MassTransit.Configuration;
using MassTransit.MessageData;
using MassTransit.MessageData.Conventions;
using MassTransit.MongoDbIntegration;
using MassTransit.MongoDbIntegration.MessageData;
using MassTransit.MongoDbIntegration.Saga;
using MassTransit.Transformation.TransformConfigurators;
using Microsoft.Extensions.Configuration;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Sol.Extensions;

namespace Solari.Callisto.Integrations.MassTransit
{
    public static class Extensions
    {
        public static ISagaRegistrationConfigurator<TSaga> MongoDbRepositoryWithCallisto<TSaga>(
            this ISagaRegistrationConfigurator<TSaga> configurator,
            IConfiguration configuration)
            where TSaga : class, IVersionedSaga
        {
            CallistoConnectorOptions options = GetCallistoConnectionOptions(configuration);

            configurator.MongoDbRepository(options.ConnectionString, repositoryConfigurator =>
            {
                repositoryConfigurator.Connection = options.ConnectionString;
                repositoryConfigurator.DatabaseName = options.Database;
            });
            return configurator;
        }

        private static CallistoConnectorOptions GetCallistoConnectionOptions(IConfiguration configuration)
        {
            IConfigurationSection section = configuration.GetSection(CallistoConstants.ConnectorAppSettingsSection);
            if (!section.Exists())
                throw new CallistoException("The callisto section does not exists in the AppSettings file.");
            var options = configuration.GetOptions<CallistoConnectorOptions>(section);
            if (string.IsNullOrEmpty(options.ConnectionString))
                throw new CallistoException("The current MassTransit integration requires a mongodb connection string");
            return options;
        }

        public static void UseMessageDataRepositoryWithCallisto(this IBusFactoryConfigurator configurator, IConfiguration
                                                                    appConfiguration)
        {
            configurator.UseMessageData(MessageDataRepositoryWithCallisto(appConfiguration));
        }

        private static IMessageDataRepository MessageDataRepositoryWithCallisto(IConfiguration configuration)
        {
            CallistoConnectorOptions options = GetCallistoConnectionOptions(configuration);
            return new MongoDbMessageDataRepository(options.ConnectionString, options.Database);
        }
    }
}
