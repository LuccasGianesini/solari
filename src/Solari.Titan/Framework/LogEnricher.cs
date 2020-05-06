using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Serilog.Context;
using Serilog.Core;

namespace Solari.Titan.Framework
{
    public class LogEnricher<T> : ILogEnricher<T> where T : class
    {
        private readonly ConcurrentStack<IDisposable> _context;

        public LogEnricher() { _context = new ConcurrentStack<IDisposable>(); }

        public void DisposeScopes()
        {
            var items = new IDisposable[_context.Count];
            _context.TryPopRange(items);

            foreach (IDisposable t in items)
                t.Dispose();
        }

        public ILogEnricher<T> WithProperties(IEnumerable<KeyValuePair<string, object>> properties)
        {
            foreach ((string key, object value) in properties) _context.Push(LogContext.PushProperty(key, value));

            return this;
        }

        public ILogEnricher<T> WithProperty(string name, object value, bool destructorObjects = false)
        {
            _context.Push(LogContext.PushProperty(name, value, destructorObjects));

            return this;
        }

        public ILogEnricher<T> WithProperty(ILogEventEnricher[] enricher)
        {
            _context.Push(LogContext.Push(enricher));

            return this;
        }

        public ILogEnricher<T> WithProperty(ILogEventEnricher enricher)
        {
            _context.Push(LogContext.Push(enricher));

            return this;
        }
    }
}