using System.Collections.Generic;
using Serilog.Context;
using Solari.Sol.Abstractions;
using Solari.Titan.Abstractions;

namespace Solari.Titan
{
    public static class LoggingScopeExtensions
    {
        public static ILoggingScope Push(this ILoggingScope scope, string key, object value, bool destructorObjects = false)
        {

            Check.ThrowIfNullOrEmpty(key, nameof(key), new TitanException($"{nameof(key)} is null or empty. Please provide a key"));
            scope.PushContext(LogContext.PushProperty(key, value, destructorObjects));
            return scope;
        }


    }
}
