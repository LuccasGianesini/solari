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
        public static ISagaRegistrationConfigurator<TSaga> CallistoSagaRepository<TSaga>(
            this ISagaRegistrationConfigurator<TSaga> configurator,
            IConfiguration configuration)
            where TSaga : class, IVersionedSaga
        {
            return SagaConfiguration.AddSagaWithCallisto(configurator, configuration);
        }


        public static void CallistoMessageDataRepository(this IBusFactoryConfigurator configurator, IConfiguration configuration)
        {
            configurator.UseMessageData(MessageDataConfiguration.MessageDataRepositoryWithCallisto(configuration));
        }


    }
}
