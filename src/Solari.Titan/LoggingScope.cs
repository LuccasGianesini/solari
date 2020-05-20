using System;
using System.Collections.Generic;
using Solari.Titan.Abstractions;

namespace Solari.Titan
{
    public class LoggingScope : ILoggingScope
    {
        private readonly Stack<IDisposable> _contexts;
        public LoggingScope() { _contexts = new Stack<IDisposable>(); }

        public static ILoggingScope OpenScope() => new LoggingScope();

        public void PushContext(IDisposable logContext)
        {
            if (logContext == null)
                throw new TitanException($"{nameof(logContext)} logContext is null.");
            _contexts.Push(logContext);
        }

        public void CloseScope() { Dispose(); }

        public void Dispose()
        {
            foreach (IDisposable disposable in _contexts)
            {
                disposable.Dispose();
            }
        }
    }
}