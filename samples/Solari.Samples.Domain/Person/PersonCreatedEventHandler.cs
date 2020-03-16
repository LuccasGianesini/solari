using System.Threading.Tasks;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Elastic.Apm;
using Elastic.Apm.Api;
using Solari.Samples.Domain.Person.Events;
using Solari.Titan;

namespace Solari.Samples.Domain.Person
{
    public class PersonCreatedEventHandler : IEventHandler<PersonCreatedEvent>
    {
        private readonly IBusPublisher _publisher;
        private readonly ITitanLogger<PersonCreatedEventHandler> _logger;
        private readonly ITracer _tracer;
        public PersonCreatedEventHandler(IBusPublisher publisher, ITitanLogger<PersonCreatedEventHandler> logger)
        {
            _publisher = publisher;
            _logger = logger;
            _tracer = Agent.Tracer;
        }
        public async Task HandleAsync(PersonCreatedEvent @event)
        {
            string spanContext = _tracer.CurrentTransaction.OutgoingDistributedTracingData.SerializeToString();
            await _publisher.PublishAsync(@event, spanContext: spanContext);
            _logger.Information("Published PersonCreatedEvent");
        }
    }
}