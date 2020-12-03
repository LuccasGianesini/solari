using MassTransit;
using MassTransit.MongoDbIntegration.Saga;
using MassTransit.Saga;
using Microsoft.Extensions.Configuration;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Callisto.Connector;
using Solari.Sol;

namespace Solari.Callisto.Integrations.MassTransit
{
    internal static class SagaConfiguration
    {
        internal static ISagaRegistrationConfigurator<TSaga> AddSagaWithCallisto<TSaga>(ISagaRegistrationConfigurator<TSaga> configurator,
                                                                                        string clientName,
                                                                                        string database,
                                                                                        IConfiguration configuration)
            where TSaga : class, ISagaVersion
        {
            if(string.IsNullOrEmpty(database))
                throw new CallistoException("Please provide a database to the SagaRepository");
            CallistoConnectorOptions options = Helper.GetCallistoConnectionOptions(clientName, configuration);
            configurator.MongoDbRepository(options.CreateUrl(Helper.GetAppOptions(configuration)).ToString(),
                                           repositoryConfigurator => { repositoryConfigurator.DatabaseName = database; });

            return configurator;
        }
    }
}
