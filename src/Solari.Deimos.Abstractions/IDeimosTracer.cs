using System;
using System.Collections.Generic;
using OpenTracing;

namespace Solari.Deimos.Abstractions
{
    public interface IDeimosTracer
    {
        ITracer OpenTracer { get; }
        ISpan TraceOperation(string operationName, Action<ISpanEnricher> enrich = null, bool createNewActivity = false);
        void FinalizeTrace(IDictionary<string, object> log = null);
        void TraceException(Exception exception);
    }
}