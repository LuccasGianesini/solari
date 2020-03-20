using System;
using System.Reflection;
using Solari.Io;

namespace Solari.Miranda.Framework
{
    public static class AttributeHelper
    {
       public static string GetExchange(Type type, string defaultNamespace)
        {
            (string @namespace, string key) = GetNamespaceAndKey(type, defaultNamespace);

            return (string.IsNullOrWhiteSpace(@namespace) ? key : $"{@namespace}").ToLowerInvariant();
        }

        public static string GetRoutingKey(Type type, string defaultNamespace)
        {
            (string @namespace, string key) = GetNamespaceAndKey(type, defaultNamespace);
            string separatedNamespace = string.IsNullOrWhiteSpace(@namespace) ? string.Empty : $"{@namespace}.";

            return $"{separatedNamespace}{key}".ToLowerInvariant();
        }

        public static int GetRetries(Type type)
        {
            MessageAttribute? attr = GetMessageAttribute(type);
            return attr?.Retries ?? 1;
        }
        public static double GetInterval(Type type)
        {
            MessageAttribute? attr = GetMessageAttribute(type);
            return attr?.Interval ?? 1;
        }
        public static string GetQueueName(string assemblyName, Type type, string defaultNamespace)
        {
            (string @namespace, string key) = GetNamespaceAndKey(type, defaultNamespace);
            var attr = GetMessageAttribute(type);
            string separatedNamespace = string.IsNullOrEmpty(attr?.Queue)
                                            ? string.IsNullOrWhiteSpace(@namespace)
                                                  ? string.Empty
                                                  : $"{@namespace}."
                                            : attr?.Queue;

            return $"{assemblyName}/{separatedNamespace}{key}".ToLowerInvariant();
        }

        public static MessageAttribute? GetMessageAttribute(Type type) { return type.GetCustomAttribute<MessageAttribute>(); }

        public static (string @namespace, string key) GetNamespaceAndKey(Type type, string defaultNamespace)
        {
            MessageAttribute? attribute = GetMessageAttribute(type);
            string @namespace = attribute?.Exchange ?? defaultNamespace;
            string key = string.IsNullOrWhiteSpace(attribute?.RoutingKey)
                             ? type.Name.Underscore()
                             : attribute.RoutingKey;

            return (@namespace, key);
        }
    }
}