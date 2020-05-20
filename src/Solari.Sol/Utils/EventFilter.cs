using System.Collections.Generic;
using System.Linq;

namespace Solari.Sol.Utils
{
    public class EventFilter : IEventFilter
    {
        public bool IsOnEventList(string eventName, IEnumerable<string> events) { return events.Any(a => a.Equals(eventName)); }
    }
}