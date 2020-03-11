using System.Threading.Tasks;

namespace Solari.Eris
{
    public interface IEventHandler<in T> where T : IEvent
    {
        Task HandleEventAsync(T @event);
    }
}