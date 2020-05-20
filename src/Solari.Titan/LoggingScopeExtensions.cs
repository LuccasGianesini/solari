using System.Collections.Generic;
using Serilog.Context;
using Solari.Titan.Abstractions;

namespace Solari.Titan
{
    public static class LoggingScopeExtensions
    {
        public static ILoggingScope Push(this ILoggingScope scope, IEnumerable<KeyValuePair<string, object>> properties)
        {
            CheckScopeValue(scope);
            return scope;
        }

        public static ILoggingScope Push(this ILoggingScope scope, string key, object value, bool destructorObjects = false)
        {
            CheckScopeValue(scope);
            CheckContextKey(key);
            scope.PushContext(LogContext.PushProperty(key, value, destructorObjects));
            return scope;
        }

        private static void CheckContextKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new TitanException($"{nameof(key)} is null or empty. Please provide a key");
        }

        private static void CheckScopeValue(ILoggingScope scope)
        {
            if (scope == null)
                throw new TitanException($"{nameof(scope)} is null. Cannot push a {nameof(LogContext)} into a null scope.");
        }
    }
}