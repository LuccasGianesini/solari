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
            ExchangeNamingConvention = type => AttributeHelper.GetExchange(type, defaultNamespace);
            RoutingKeyConvention = type => AttributeHelper.GetRoutingKey(type, defaultNamespace);
            QueueNamingConvention = type => AttributeHelper.GetQueueName(assemblyName, type, defaultNamespace);
            ErrorExchangeNamingConvention = () => $"{defaultNamespace}.error";
            RetryLaterExchangeConvention = span => $"{defaultNamespace}.retry";
            RetryLaterQueueNameConvetion = (exchange, span) 
                => $"{defaultNamespace}.retry_for_{exchange.Replace(".", "_")}_in_{span.TotalMilliseconds}_ms".ToLowerInvariant();
            
        }

    
    }
}