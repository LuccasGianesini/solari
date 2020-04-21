using System.Threading.Tasks;
using Solari.Eris;
using Solari.Miranda;
using Solari.Titan;

namespace Solari.Samples.Domain.Person.Events.Handlers
{
    public class PersonCreatedEventHandler : IEventHandler<PersonCreatedEvent>
    {
        private readonly IMirandaClient _publisher;
        private readonly ITitanLogger<PersonCreatedEventHandler> _logger;
        public PersonCreatedEventHandler(IMirandaClient publisher, ITitanLogger<PersonCreatedEventHandler> logger)
        {
            _publisher = publisher;
            _logger = logger;
        }

        public async Task HandleEventAsync(PersonCreatedEvent @event)
        {
            await _publisher.PublishAsync(@event);
            _logger.Information($"Published '{PersonConstants.CreatePersonOperationName} event'");
        }
    }
}