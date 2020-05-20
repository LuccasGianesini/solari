using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Solari.Eris
{
    public class Dispatcher : IDispatcher
    {
        private readonly IServiceScopeFactory _factory;

        public Dispatcher(IServiceScopeFactory factory) { _factory = factory; }

        public async IAsyncEnumerable<TCommandResult> DispatchCommand<TCommand, TCommandResult>(TCommand command) where TCommand : class, ICommand
        {
            using (IServiceScope scope = _factory.CreateScope())
            {
                IEnumerable<ICommandHandler<TCommand, TCommandResult>> handlers = scope
                                                                                  .ServiceProvider
                                                                                  .GetServices<ICommandHandler<TCommand, TCommandResult>>();
                foreach (ICommandHandler<TCommand, TCommandResult> commandHandler in handlers) yield return await commandHandler.HandleCommandAsync(command);
            }
        }

        public async Task DispatchCommand<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            using (IServiceScope scope = _factory.CreateScope())
            {
                IEnumerable<ICommandHandler<TCommand>> handlers = scope.ServiceProvider.GetServices<ICommandHandler<TCommand>>();
                foreach (ICommandHandler<TCommand> handler in handlers) await handler.HandleCommandAsync(command);
            }
        }

        public async Task DispatchEvent<TEvent>(TEvent @event) where TEvent : class, IEvent
        {
            using (IServiceScope scope = _factory.CreateScope())
            {
                IEnumerable<IEventHandler<TEvent>> handlers = scope.ServiceProvider.GetServices<IEventHandler<TEvent>>();
                foreach (IEventHandler<TEvent> handler in handlers) await handler.HandleEventAsync(@event);
            }
        }

        public async IAsyncEnumerable<TQueryResult> DispatchQuery<TQuery, TQueryResult>(TQuery query) where TQuery : class, IQuery<TQueryResult>
        {
            using (IServiceScope scope = _factory.CreateScope())
            {
                IEnumerable<IQueryHandler<TQuery, TQueryResult>> handlers = scope.ServiceProvider.GetServices<IQueryHandler<TQuery, TQueryResult>>();
                foreach (IQueryHandler<TQuery, TQueryResult> queryHandler in handlers) yield return await queryHandler.HandleQueryAsync(query);
            }
        }
    }
}