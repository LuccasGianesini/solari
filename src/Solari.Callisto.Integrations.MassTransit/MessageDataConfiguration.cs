using MassTransit.MessageData;
using MassTransit.MongoDbIntegration.MessageData;
using Microsoft.Extensions.Configuration;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts;
using Solari.Callisto.Connector;

namespace Solari.Callisto.Integrations.MassTransit
{
    internal static class MessageDataConfiguration
    {
        public static IMessageDataRepository MessageDataRepositoryWithCallisto(string clientName, string database)
        {
            ICallistoClient client = CallistoClientRegistry.Instance.GetClient(clientName);
            return new MongoDbMessageDataRepository(client.ConnectionString, database);
        }
    }
}
