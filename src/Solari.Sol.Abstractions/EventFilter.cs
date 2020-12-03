using System.Collections.Generic;
using System.Linq;

namespace Solari.Sol.Abstractions
{
    public class EventFilter : IEventFilter
    {
        public bool IsOnEventList(string eventName, IEnumerable<string> events) { return events.Any(a => a.Equals(eventName)); }
    }
}
