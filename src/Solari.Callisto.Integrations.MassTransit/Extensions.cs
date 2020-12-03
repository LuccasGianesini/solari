using MassTransit;
using MassTransit.Configuration;
using MassTransit.MessageData;
using MassTransit.MessageData.Conventions;
using MassTransit.MongoDbIntegration;
using MassTransit.MongoDbIntegration.MessageData;
using MassTransit.MongoDbIntegration.Saga;
using MassTransit.Saga;
using MassTransit.Transformation.TransformConfigurators;
using Microsoft.Extensions.Configuration;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Exceptions;

namespace Solari.Callisto.Integrations.MassTransit
{
    public static class Extensions
    {
        public static ISagaRegistrationConfigurator<TSaga> CallistoSagaRepository<TSaga>(this ISagaRegistrationConfigurator<TSaga> configurator,
                                                                                         string clientName,
                                                                                         string database,
                                                                                         IConfiguration configuration)
            where TSaga : class, ISagaVersion
        {
            return SagaConfiguration.AddSagaWithCallisto(configurator, clientName, database, configuration);
        }


        public static void CallistoMessageDataRepository(this IBusFactoryConfigurator configurator,
                                                         string clientName,
                                                         string database,
                                                         IConfiguration configuration)
        {
            configurator.UseMessageData(MessageDataConfiguration.MessageDataRepositoryWithCallisto(clientName, database, configuration));
        }
    }
}
