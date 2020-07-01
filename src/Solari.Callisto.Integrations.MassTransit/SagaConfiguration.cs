using MassTransit;
using MassTransit.MongoDbIntegration.Saga;
using Microsoft.Extensions.Configuration;
using Solari.Callisto.Abstractions;

namespace Solari.Callisto.Integrations.MassTransit
{
    internal static class SagaConfiguration
    {
        internal static ISagaRegistrationConfigurator<TSaga> AddSagaWithCallisto<TSaga>(ISagaRegistrationConfigurator<TSaga> configurator, IConfiguration configuration)
            where TSaga : class, IVersionedSaga
        {
            CallistoConnectorOptions options = Helper.GetCallistoConnectionOptions(configuration);

            if (options.MassTransitStorageConfiguration is null)
            {
                ConfigureSagaRepository(configurator, options.ConnectionString, options.Database);
            }
            else
            {
                string db = string.IsNullOrEmpty(options.MassTransitStorageConfiguration.SagasDb)
                                ? options.Database
                                : options.MassTransitStorageConfiguration.SagasDb;
                ConfigureSagaRepository(configurator, options.ConnectionString, db);
            }

            return configurator;
        }

        private static void ConfigureSagaRepository<TSaga>(ISagaRegistrationConfigurator<TSaga> configurator,
                                                           string connectionString,
                                                           string database)
            where TSaga : class, IVersionedSaga
        {
            configurator.MongoDbRepository(connectionString, repositoryConfigurator =>
            {
                repositoryConfigurator.Connection = connectionString;
                repositoryConfigurator.DatabaseName = database;
            });
        }
    }
}
