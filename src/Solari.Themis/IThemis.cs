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
        ISpan TraceError(string errorMessage, LogLevel level = LogLevel.Error, params object[] args);
        ISpan TraceException(Exception exception, string logMessage = null, LogLevel level = LogLevel.Critical, params object[] args);
    }
}
