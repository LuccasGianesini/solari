using System.Threading.Tasks;
using Solari.Eris;
using Solari.Titan;

namespace Solari.Samples.Domain.Person.Events.Handlers
{
    public class PersonCreatedEventHandler : IEventHandler<PersonCreatedEvent>
    {
        private readonly ITitanLogger<PersonCreatedEventHandler> _logger;
        public PersonCreatedEventHandler(ITitanLogger<PersonCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleEventAsync(PersonCreatedEvent @event)
        {
            _logger.Information($"Published '{PersonConstants.CreatePersonOperationName} event'");
        }
    }
}