using System;
using System.Collections.Generic;
using OpenTracing;
using OpenTracing.Tag;
using Solari.Deimos.Abstractions;

namespace Solari.Deimos
{
    public sealed class SpanEnricher : ISpanEnricher
    {
        private readonly ISpan _span;

        public SpanEnricher(ISpan span) { _span = span ?? throw new ArgumentNullException(nameof(span)); }

        public ISpanEnricher Tag(string key, string value)
        {
            _span.SetTag(key, value);
            return this;
        }

        public ISpanEnricher Tag(string key, bool value)
        {
            _span.SetTag(key, value);
            return this;
        }

        public ISpanEnricher Tag(string key, int value)
        {
            _span.SetTag(key, value);
            return this;
        }

        public ISpanEnricher Tag(string key, double value)
        {
            _span.SetTag(key, value);
            return this;
        }

        public ISpanEnricher Tag(StringTag key, string value)
        {
            _span.SetTag(key, value);
            return this;
        }

        public ISpanEnricher Tag(BooleanTag key, bool value)
        {
            _span.SetTag(key, value);
            return this;
        }

        public ISpanEnricher Tag(IntTag key, int value)
        {
            _span.SetTag(key, value);
            return this;
        }

        public ISpanEnricher Tag(IntOrStringTag key, string value)
        {
            _span.SetTag(key, value);
            return this;
        }

        public ISpanEnricher BaggageItem(string key, string value)
        {
            _span.SetBaggageItem(key, value);
            return this;
        }

        public ISpanEnricher Log(string @event)
        {
            _span.Log(@event);
            return this;
        }

        public ISpanEnricher Log(DateTimeOffset timestamp, string @event)
        {
            _span.Log(timestamp, @event);
            return this;
        }

        public ISpanEnricher Log(DateTimeOffset timestamp, IEnumerable<KeyValuePair<string, object>> fields)
        {
            _span.Log(timestamp, fields);
            return this;
        }

        public ISpanEnricher Log(IEnumerable<KeyValuePair<string, object>> fields)
        {
            _span.Log(fields);
            return this;
        }
    }
}