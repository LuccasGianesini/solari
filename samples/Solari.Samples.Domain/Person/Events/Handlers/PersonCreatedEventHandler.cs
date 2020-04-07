using System.Threading.Tasks;
using Elastic.Apm;
using Elastic.Apm.Api;
using Solari.Deimos.CorrelationId;
using Solari.Eris;
using Solari.Miranda;
using Solari.Titan;
using ITracer = OpenTracing.ITracer;

namespace Solari.Samples.Domain.Person.Events.Handlers
{
    public class PersonCreatedEventHandler : IEventHandler<PersonCreatedEvent>
    {
        private readonly IMirandaClient _publisher;
        private readonly ITitanLogger<PersonCreatedEventHandler> _logger;
        private readonly ICorrelationContextAccessor _contextAccessor;
        private readonly ITracer _tracer;

        public PersonCreatedEventHandler(IMirandaClient publisher, ITitanLogger<PersonCreatedEventHandler> logger, 
                                         ICorrelationContextAccessor contextAccessor, ITracer tracer)
        {
            _publisher = publisher;
            _logger = logger;
            _contextAccessor = contextAccessor;
            _tracer = tracer;
        }
        public async Task HandleEventAsync(PersonCreatedEvent @event)
        {
            ICorrelationContext current = _contextAccessor.Current;
            
            
            await _publisher.PublishAsync(@event);
            _logger.Information($"Published '{PersonConstants.CreatePersonOperationName} event'");
        }
    }
}