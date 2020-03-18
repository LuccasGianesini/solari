using System.Threading.Tasks;

namespace Solari.Eris
{
    public interface IEventHandler<in T> : Convey.CQRS.Events.IEventHandler<T> where T :class, IEvent
    {
        Task HandleEventAsync(T @event);
    }
}