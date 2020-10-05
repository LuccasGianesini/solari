using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace Solari.Themis
{
    public interface IThemis<T>
    {
        ILogger<T> Logger { get; }
        ITracer Tracer { get; }
        ISpan TraceOperation(string operationName);
        ISpan TraceError(string logMessage, IDictionary<string, object> spanLog = null, LogLevel level = LogLevel.Error, params object[] args);
        ISpan TraceException(Exception exception, string logMessage = null, LogLevel level = LogLevel.Critical, params object[] args);

        bool TraceAndPropagate(Exception exception, string logMessage = null, LogLevel level = LogLevel.Critical, Action<ISpan> spanAction = null,
                               Action action = null, params object[] args);

        bool TraceAndHandle(Exception exception, string logMessage = null, LogLevel level = LogLevel.Critical, Action<ISpan> spanAction = null,
                            Action action = null, params object[] args);
    }
}
