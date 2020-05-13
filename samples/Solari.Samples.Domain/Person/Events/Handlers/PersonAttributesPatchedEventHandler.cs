using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenTracing;
using Solari.Eris;
using Solari.Titan;

namespace Solari.Samples.Domain.Person.Events.Handlers
{
    public class PersonAttributesPatchedEventHandler : IEventHandler<PersonAttributesPatchedEvent>
    {
        private readonly ILogger<PersonAttributesPatchedEventHandler> _logger;
        private readonly ITracer _tracer;

        public async Task HandleEventAsync(PersonAttributesPatchedEvent @event) { }
    }
}