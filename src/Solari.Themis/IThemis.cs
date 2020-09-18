using System;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace Solari.Themis
{
    public interface IThemis<T>
    {
        ILogger<T> Logger { get; }
        ITracer Tracer { get; }
        ISpan TraceOperation(string operationName);
        ISpan TraceException(string operationName,Exception exception, string logMessage = null, LogLevel level = LogLevel.Error);
    }
}
