using System.Threading.Tasks;
using OpenTracing;
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

        public PersonAttributesPatchedEventHandler()
        {
           
        }

        public async Task HandleEventAsync(PersonAttributesPatchedEvent @event) { }
    }
}