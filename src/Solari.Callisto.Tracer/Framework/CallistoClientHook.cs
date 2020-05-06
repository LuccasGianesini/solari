using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using Solari.Callisto.Connector;
using Solari.Deimos.Abstractions;

namespace Solari.Callisto.Tracer.Framework
{
    public interface ICallistoClientHook
    {
        void AddHook();
    }

    public class CallistoClientHook : ICallistoClientHook
    {
        private readonly ICallistoEventListener _callistoEventListener;
        private readonly ICallistoConnection _connection;
        private readonly IOptions<DeimosOptions> _deimosOptions;
        private readonly ICallistoConnectionFactory _factory;
        private readonly IOptions<CallistoTracerOptions> _options;

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
            if (!_deimosOptions.Value.TracingEnabled || !_options.Value.Enabled) return;
            MongoClientSettings currentSettings = _connection.GetClient().Settings.Clone();

            currentSettings.ClusterConfigurator = builder =>
            {
                builder.Subscribe<CommandStartedEvent>(_callistoEventListener.StartEventHandler)
                       .Subscribe<CommandSucceededEvent>(_callistoEventListener.SuccessEventHandler)
                       .Subscribe<CommandFailedEvent>(_callistoEventListener.ErrorEventHandler);
            };
            _connection.UpdateClient(_factory.Make(currentSettings, _connection.DataBaseName).GetClient());
        }
    }
}