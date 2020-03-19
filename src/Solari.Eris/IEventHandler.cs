using System.Threading.Tasks;

namespace Solari.Eris
{
    public interface IEventHandler<in T> where T : class, IEvent
    {
        Task HandleEventAsync(T @event);
    }
}