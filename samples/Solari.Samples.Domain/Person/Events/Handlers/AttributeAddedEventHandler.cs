using System.Threading.Tasks;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Elastic.Apm;
using Elastic.Apm.Api;
using Solari.Titan;

namespace Solari.Samples.Domain.Person.Events.Handlers
{
    public class AttributeAddedEventHandler : IEventHandler<AttributeAddedEvent>
    {
        private readonly IBusPublisher _publisher;
        private readonly ITitanLogger<AttributeAddedEventHandler> _logger;
        private readonly ITracer _tracer;

        public AttributeAddedEventHandler(IBusPublisher publisher, ITitanLogger<AttributeAddedEventHandler> logger)
        {
            _publisher = publisher;
            _logger = logger;
            _tracer = Agent.Tracer;
        }

        public async Task HandleAsync(AttributeAddedEvent @event)
        {
            string spanContext = _tracer.CurrentTransaction.OutgoingDistributedTracingData.SerializeToString();
            await _publisher.PublishAsync(@event, spanContext);
            _logger.Information($"Published '{PersonConstants.AddAttributeToPersonOperationName}'");
        }
    }
}