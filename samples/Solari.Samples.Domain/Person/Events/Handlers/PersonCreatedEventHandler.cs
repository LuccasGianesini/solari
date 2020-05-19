using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Solari.Eris;
using Solari.Titan;

namespace Solari.Samples.Domain.Person.Events.Handlers
{
    public class PersonCreatedEventHandler : IEventHandler<PersonCreatedEvent>
    {
        private readonly ILogger<PersonCreatedEventHandler> _logger;

        public PersonCreatedEventHandler(ILogger<PersonCreatedEventHandler> logger) { _logger = logger; }

        public async Task HandleEventAsync(PersonCreatedEvent @event) { _logger.LogInformation($"Published '{PersonConstants.CreatePersonOperationName} event'"); }
    }
}