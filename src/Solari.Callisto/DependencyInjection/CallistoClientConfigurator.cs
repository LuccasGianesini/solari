using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Callisto.Connector;
using Solari.Callisto.Tracer.Framework;
using Solari.Sol;
using Solari.Sol.Extensions;

namespace Solari.Callisto.DependencyInjection
{
    public class CallistoClientConfigurator : ICallistoClientConfigurator
    {
        private readonly ISolariBuilder _builder;
        private readonly IEnumerable<CallistoConnectorOptions> _clients;

        public CallistoClientConfigurator(ISolariBuilder builder, IEnumerable<CallistoConnectorOptions> clients)
        {
            _builder = builder;
            _clients = clients;
        }

        public ICallistoClientConfigurator RegisterClient(string clientName, ICallistoClient client,
                                                          Action<ICallistoCollectionConfigurator> configureCollections)
        {
            if(string.IsNullOrEmpty(clientName))
                throw new CallistoException("Please provide the name of the client that should be registered.");
            if(client is null)
                throw new CallistoException("Please provide a valid instance of a 'ICallistoClient'");
            _builder.AddBuildAction(new BuildAction($"Registering Callisto client with name: {clientName}")
            {
                Action = provider =>
                {
                    var registry = provider.GetRequiredService<ICallistoClientRegistry>();
                    registry.AddClient(clientName, client);
                }

            });
            configureCollections?.Invoke(new CallistoCollectionConfigurator(_builder, clientName));
            return this;
        }
        public ICallistoClientConfigurator RegisterClient(string clientName, Action<ICallistoCollectionConfigurator> configureCollections)
        {

            if(string.IsNullOrEmpty(clientName))
                throw new CallistoException("Please provide the name of the client that should be registered.");

            _builder.AddBuildAction(new BuildAction($"Registering Callisto client with name: {clientName}")
            {
                Action = provider =>
                {
                    CallistoConnectorOptions clientOptions = _clients.FirstOrDefault(a => a.Name.ToUpperInvariant()
                                                                                           .Equals(clientName.ToUpperInvariant()));

                    if(clientOptions is null)
                        throw new CallistoException($"The options for client '{clientName}' was not found. Check the AppSettings file.");

                    var registry = provider.GetRequiredService<ICallistoClientRegistry>();
                    var eventListener = provider.GetRequiredService<ICallistoEventListener>();
                    ApplicationOptions appOptions = _builder.Configuration.GetApplicationOptions();
                    registry.AddClient(clientName, clientOptions.CreateUrl(appOptions)
                                                                .CreateMongoClientSettings()
                                                                .ConfigureTracing(clientOptions.Trace, eventListener)
                                                                .CreateMongoClient()
                                                                .CreateCallistoClient());
                }
            });
            configureCollections?.Invoke(new CallistoCollectionConfigurator(_builder, clientName));
            return this;
        }
    }
}
