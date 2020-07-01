using MassTransit.MessageData;
using MassTransit.MongoDbIntegration.MessageData;
using Microsoft.Extensions.Configuration;
using Solari.Callisto.Abstractions;

namespace Solari.Callisto.Integrations.MassTransit
{
    internal static class MessageDataConfiguration
    {
        public static IMessageDataRepository MessageDataRepositoryWithCallisto(IConfiguration configuration)
        {
            CallistoConnectorOptions options = Helper.GetCallistoConnectionOptions(configuration);
            if (options.MassTransitStorageConfiguration is null)
            {
                return new MongoDbMessageDataRepository(options.ConnectionString, options.Database);
            }

            string db = string.IsNullOrEmpty(options.MassTransitStorageConfiguration.MessageDataDb)
                            ? options.Database
                            : options.MassTransitStorageConfiguration.MessageDataDb;
            return new MongoDbMessageDataRepository(options.ConnectionString, db);
        }
    }
}
