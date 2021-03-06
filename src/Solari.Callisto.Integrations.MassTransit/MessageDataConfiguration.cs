using System.Collections.Generic;
using System.Linq;
using MassTransit.MessageData;
using MassTransit.MongoDbIntegration.MessageData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OpenTracing.Util;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Callisto.Connector;
using Solari.Callisto.Tracer;
using Solari.Callisto.Tracer.Framework;
using Solari.Sol;
using Solari.Sol.Extensions;
using Solari.Sol.Utils;

namespace Solari.Callisto.Integrations.MassTransit
{
    internal static class MessageDataConfiguration
    {
        public static IMessageDataRepository MessageDataRepositoryWithCallisto(string clientName, string database,
                                                                               IConfiguration configuration)
        {
            if(string.IsNullOrEmpty(database))
                throw new CallistoException("Please provide a database to the SagaRepository");
            CallistoConnectorOptions options = Helper.GetCallistoConnectionOptions(clientName, configuration);
            return new MongoDbMessageDataRepository(options.CreateUrl(Helper.GetAppOptions(configuration)).ToString(), database);
        }



    }
}
