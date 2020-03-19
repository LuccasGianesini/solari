using System;
using System.Reflection;
using RawRabbit.Common;
using Solari.Io;

namespace Solari.Miranda.Framework
{
    /// <summary>
    /// FROM CONVEY see: https://convey-stack.github.io/
    /// </summary>
    public sealed class CustomNamingConventions : NamingConventions
    {
        public CustomNamingConventions(string defaultNamespace)
        {
            string? assemblyName = Assembly.GetEntryAssembly()?.GetName().Name;
            ExchangeNamingConvention = type => GetExchange(type, defaultNamespace);
            RoutingKeyConvention = type => GetRoutingKey(type, defaultNamespace);
            QueueNamingConvention = type => GetQueueName(assemblyName, type, defaultNamespace);
            ErrorExchangeNamingConvention = () => $"{defaultNamespace}.error";
            RetryLaterExchangeConvention = span => $"{defaultNamespace}.retry";
            RetryLaterQueueNameConvetion = (exchange, span) =>
                $"{defaultNamespace}.retry_for_{exchange.Replace(".", "_")}_in_{span.TotalMilliseconds}_ms"
                    .ToLowerInvariant();
            
        }

        private static string GetExchange(Type type, string defaultNamespace)
        {
            (string @namespace, string key) = GetNamespaceAndKey(type, defaultNamespace);

            return (string.IsNullOrWhiteSpace(@namespace) ? key : $"{@namespace}").ToLowerInvariant();
        }

        private static string GetRoutingKey(Type type, string defaultNamespace)
        {
            (string @namespace, string key) = GetNamespaceAndKey(type, defaultNamespace);
            string separatedNamespace = string.IsNullOrWhiteSpace(@namespace) ? string.Empty : $"{@namespace}.";

            return $"{separatedNamespace}{key}".ToLowerInvariant();
        }

        private static string GetQueueName(string assemblyName, Type type, string defaultNamespace)
        {
            (string @namespace, string key) = GetNamespaceAndKey(type, defaultNamespace);
            string separatedNamespace = string.IsNullOrWhiteSpace(@namespace) ? string.Empty : $"{@namespace}.";

            return $"{assemblyName}/{separatedNamespace}{key}".ToLowerInvariant();
        }

        private static (string @namespace, string key) GetNamespaceAndKey(Type type, string defaultNamespace)
        {
            MessageAttribute? attribute = type.GetCustomAttribute<MessageAttribute>();
            string @namespace = attribute?.Exchange ?? defaultNamespace;
            string key = string.IsNullOrWhiteSpace(attribute?.RoutingKey)
                             ? type.Name.Underscore()
                             : attribute.RoutingKey;

            return (@namespace, key);
        }
    }
}