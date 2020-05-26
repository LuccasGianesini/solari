using System;
using Automatonymous;
using MassTransit.MongoDbIntegration.Saga;
using Solari.Samples.Domain.Person.Events;
using Solari.Sol.Extensions;

namespace Solari.Samples.Application.Person
{
    public class PersonStateMachine : MassTransitStateMachine<PersonState>
    {
        public PersonStateMachine()
        {
            Event(() => OnPersonCreated, x => x.CorrelateById(a => a.Message.Result.Id));
            InstanceState(x => x.CurrentState);
            Initially(
                      When(OnPersonCreated).TransitionTo(PersonCreated));
        }
        public State PersonCreated { get; set; }
        public Event<IPersonCreatedEvent> OnPersonCreated { get; private set; }
    }

    public class PersonState : SagaStateMachineInstance, IVersionedSaga
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public int Version { get; set; }
    }
}