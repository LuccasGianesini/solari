using System.Collections.Generic;

namespace Solari.Sol.Abstractions
{
    public interface IEventFilter
    {
        /// <summary>
        ///     Checks if the <see cref="IEnumerable{T}" /> contains the event name.
        /// </summary>
        /// <param name="eventName">Name of the event</param>
        /// <param name="events"><see cref="IEnumerable{T}" /> of events</param>
        /// <returns>True if the event is on the list. </returns>
        bool IsOnEventList(string eventName, IEnumerable<string> events);
    }
}
