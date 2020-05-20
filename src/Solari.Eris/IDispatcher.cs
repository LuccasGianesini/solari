using System.Collections.Generic;
using System.Threading.Tasks;

namespace Solari.Eris
{
    public interface IDispatcher
    {
        IAsyncEnumerable<TCommandResult> DispatchCommand<TCommand, TCommandResult>(TCommand command) where TCommand : class, ICommand;
        Task DispatchCommand<TCommand>(TCommand command) where TCommand : class, ICommand;
        Task DispatchEvent<TEvent>(TEvent @event) where TEvent : class, IEvent;
        IAsyncEnumerable<TQueryResult> DispatchQuery<TQuery, TQueryResult>(TQuery query) where TQuery : class, IQuery<TQueryResult>;
    }
}