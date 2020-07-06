using MassTransit;
using MassTransit.MongoDbIntegration.Saga;
using MassTransit.Saga;
using Microsoft.Extensions.Configuration;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts;
using Solari.Callisto.Connector;

namespace Solari.Callisto.Integrations.MassTransit
{
    internal static class SagaConfiguration
    {
        internal static ISagaRegistrationConfigurator<TSaga> AddSagaWithCallisto<TSaga>(ISagaRegistrationConfigurator<TSaga> configurator,
                                                                                        string clientName,
                                                                                        string database)
            where TSaga : class, ISagaVersion
        {
            ICallistoClient client = CallistoClientRegistry.Instance.GetClient(clientName);
            configurator.MongoDbRepository(client.ConnectionString,
                                           repositoryConfigurator => { repositoryConfigurator.DatabaseName = database; });

            return configurator;
        }
    }
}
