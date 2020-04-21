using System;
using System.Collections.Generic;
using OpenTracing;

namespace Solari.Deimos.Abstractions
{
    public interface IDeimosTracer
    {
        ISpan TraceOperation(string operationName, Action<ISpanEnricher> enrich = null);
        void FinalizeTrace(IDictionary<string, object> log = null);
        void TraceException(Exception exception);
    }
}