using System.Threading.Tasks;
using Elastic.Apm;
using Elastic.Apm.Api;
using Solari.Eris;
using Solari.Miranda;
using Solari.Titan;

namespace Solari.Samples.Domain.Person.Events.Handlers
{
    public class PersonCreatedEventHandler : IEventHandler<PersonCreatedEvent>
    {
        private readonly MirandaClient _publisher;
        private readonly ITitanLogger<PersonCreatedEventHandler> _logger;
        private readonly ITracer _tracer;
        public PersonCreatedEventHandler(MirandaClient publisher, ITitanLogger<PersonCreatedEventHandler> logger)
        {
            _publisher = publisher;
            _logger = logger;
            _tracer = Agent.Tracer;
        }
        public async Task HandleEventAsync(PersonCreatedEvent @event)
        {
            string spanContext = _tracer.CurrentTransaction.OutgoingDistributedTracingData.SerializeToString();
            // await _publisher.PublishAsync(@event);
            _logger.Information($"Published '{PersonConstants.CreatePersonOperationName} event'");
        }
    }
}