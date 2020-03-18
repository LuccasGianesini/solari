using System.Threading.Tasks;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Elastic.Apm;
using Elastic.Apm.Api;
using Solari.Titan;

namespace Solari.Samples.Domain.Person.Events.Handlers
{
    public class PersonAttributesPatchedEventHandler : IEventHandler<PersonAttributesPatchedEvent>
    {
        private readonly IBusPublisher _publisher;
        private readonly ITitanLogger<PersonAttributesPatchedEventHandler> _logger;
        private readonly ITracer _tracer;

        public PersonAttributesPatchedEventHandler(IBusPublisher publisher, ITitanLogger<PersonAttributesPatchedEventHandler> logger)
        {
            _publisher = publisher;
            _logger = logger;
            _tracer = Agent.Tracer;
        }

        public async Task HandleAsync(PersonAttributesPatchedEvent @event)
        {
            string spanContext = _tracer.CurrentTransaction.OutgoingDistributedTracingData.SerializeToString();
            await _publisher.PublishAsync(@event, spanContext: spanContext);
            _logger.Information($"Published 'person-attributes-patched' event");
        }
    }
}