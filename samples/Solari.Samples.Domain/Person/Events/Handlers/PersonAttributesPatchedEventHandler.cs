using System.Threading.Tasks;
using Elastic.Apm;
using Elastic.Apm.Api;
using Solari.Eris;
using Solari.Miranda;
using Solari.Titan;

namespace Solari.Samples.Domain.Person.Events.Handlers
{
    public class PersonAttributesPatchedEventHandler : IEventHandler<PersonAttributesPatchedEvent>
    {
        private readonly IMirandaClient _publisher;
        private readonly ITitanLogger<PersonAttributesPatchedEventHandler> _logger;
        private readonly ITracer _tracer;

        public PersonAttributesPatchedEventHandler(IMirandaClient publisher, ITitanLogger<PersonAttributesPatchedEventHandler> logger)
        {
            _publisher = publisher;
            _logger = logger;
            _tracer = Agent.Tracer;
        }

        public async Task HandleEventAsync(PersonAttributesPatchedEvent @event)
        {
            string spanContext = _tracer.CurrentTransaction.OutgoingDistributedTracingData.SerializeToString();
            // await _publisher.PublishAsync(@event, spanContext: spanContext);
            _logger.Information($"Published 'person-attributes-patched' event");
        }
    }
}