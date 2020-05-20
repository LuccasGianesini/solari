using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace Solari.Themis
{
    public interface IThemis
    {
        ISpan TraceOperation(string operationName);
        void TraceException(Exception exception, string customMessage = null, LogLevel level = LogLevel.Error);
    }
}