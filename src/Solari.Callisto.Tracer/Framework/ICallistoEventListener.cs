using MongoDB.Driver.Core.Events;

namespace Solari.Callisto.Tracer.Framework
{
    public interface ICallistoEventListener
    {
        void StartEventHandler(CommandStartedEvent @event);
        void SuccessEventHandler(CommandSucceededEvent @event);
        void ErrorEventHandler(CommandFailedEvent @event);
    }
}