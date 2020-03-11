using System;
using System.Collections.Generic;
using OpenTracing.Tag;

namespace Solari.Deimos.Jaeger
{
    public interface ISpanEnricher
    {
        /// <summary>
        /// Tag the current active span.
        /// </summary>
        /// <param name="key">Tag key</param>
        /// <param name="value">Tag value</param>
        /// <returns></returns>
        ISpanEnricher Tag(string key, string value);

        /// <summary>
        /// Tag the current active span.
        /// </summary>
        /// <param name="key">Tag key</param>
        /// <param name="value">Tag value</param>
        /// <returns></returns>
        ISpanEnricher Tag(string key, bool value);

        /// <summary>
        /// Tag the current active span.
        /// </summary>
        /// <param name="key">Tag key</param>
        /// <param name="value">Tag value</param>
        /// <returns></returns>
        ISpanEnricher Tag(string key, int value);

        /// <summary>
        /// Tag the current active span.
        /// </summary>
        /// <param name="key">Tag key</param>
        /// <param name="value">Tag value</param>
        /// <returns></returns>
        ISpanEnricher Tag(string key, double value);

        /// <summary>
        /// Tag the current active span.
        /// </summary>
        /// <param name="key">Tag key</param>
        /// <param name="value">Tag value</param>
        /// <returns></returns>
        ISpanEnricher Tag(StringTag key, string value);

        /// <summary>
        /// Tag the current active span.
        /// </summary>
        /// <param name="key">Tag key</param>
        /// <param name="value">Tag value</param>
        /// <returns></returns>
        ISpanEnricher Tag(BooleanTag key, bool value);
        
        /// <summary>
        /// Tag the current active span.
        /// </summary>
        /// <param name="key">Tag key</param>
        /// <param name="value">Tag value</param>
        /// <returns></returns>
        ISpanEnricher Tag(IntTag key, int value);
        /// <summary>
        /// Tag the current active span.
        /// </summary>
        /// <param name="key">Tag key</param>
        /// <param name="value">Tag value</param>
        /// <returns></returns>
        ISpanEnricher Tag(IntOrStringTag key, string value);

        /// <summary>
        /// Set a baggage item into the currently active span.
        /// </summary>
        /// <param name="key">Baggage key</param>
        /// <param name="value">Baggage item</param>
        /// <returns></returns>
        ISpanEnricher BaggageItem(string key, string value);

        /// <summary>
        /// Log into the currently active span.
        /// </summary>
        /// <param name="event">Event</param>
        /// <returns></returns>
        ISpanEnricher Log(string @event);
        
        /// <summary>
        /// Log into the currently active span.
        /// </summary>
        /// <param name="timestamp">Event timestamp</param>
        /// <param name="event">Event</param>
        /// <returns></returns>
        ISpanEnricher Log(DateTimeOffset timestamp, string @event);
        
        /// <summary>
        /// Log into the currently active span.
        /// </summary>
        /// <param name="timestamp">Event timestamp</param>
        /// <param name="fields">Fields</param>
        /// <returns></returns>
        ISpanEnricher Log(DateTimeOffset timestamp, IEnumerable<KeyValuePair<string, object>> fields);
        
        /// <summary>
        /// Log into the currently active span.
        /// </summary>
        /// <param name="fields">Fields</param>
        /// <returns></returns>
        ISpanEnricher Log(IEnumerable<KeyValuePair<string, object>> fields);
    }
}