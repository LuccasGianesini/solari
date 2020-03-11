﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using Solari.Callisto.Connector;
using Solari.Deimos.Abstractions;
using Solari.Rhea;

namespace Solari.Callisto.Tracer.Framework
{
    public interface ICallistoClientHook
    {
        void AddHook();
    }

    public class CallistoClientHook : ICallistoClientHook
    {
        private readonly ICallistoConnection _connection;
        private readonly ICallistoConnectionFactory _factory;
        private readonly IOptions<CallistoTracerOptions> _options;
        private readonly IOptions<DeimosOptions> _deimosOptions;
        private readonly ICallistoEventListener _callistoEventListener;
        
        public CallistoClientHook(ICallistoConnection connection, 
                                  ICallistoConnectionFactory factory, 
                                  IOptions<CallistoTracerOptions> options, 
                                  IOptions<DeimosOptions> deimosOptions,
                                  ICallistoEventListener callistoEventListener)
        {
            _connection = connection;
            _factory = factory;
            _options = options;
            _deimosOptions = deimosOptions;
            _callistoEventListener = callistoEventListener;
        }

        public void AddHook()
        {
            // Log.Information("Configuring callisto hook!");
            if (!_deimosOptions.Value.TracingEnabled || !_options.Value.Enabled) return;
            MongoClientSettings currentSettings = _connection.GetClient().Settings.Clone();
            
            currentSettings.ClusterConfigurator = builder =>
            {
                // Log.Debug("Subscribing to mongodb events");
                builder.Subscribe<CommandStartedEvent>(_callistoEventListener.StartEventHandler)
                       .Subscribe<CommandSucceededEvent>(_callistoEventListener.SuccessEventHandler)
                       .Subscribe<CommandFailedEvent>(_callistoEventListener.ErrorEventHandler);
            };
            _connection.UpdateClient(_factory.Make(currentSettings, _connection.DataBaseName).GetClient());
        }
    }
}