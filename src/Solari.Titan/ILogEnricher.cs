using System.Collections.Generic;
using Serilog.Core;

namespace Solari.Titan
{
    public interface ILogEnricher<T> where T : class
    {
        /// <summary>
        ///     This method is called automatically.
        /// </summary>
        void DisposeScopes();

        ILogEnricher<T> WithProperties(IEnumerable<KeyValuePair<string, object>> properties);
        ILogEnricher<T> WithProperty(string name, object value, bool destructorObjects = false);
        ILogEnricher<T> WithProperty(ILogEventEnricher[] enricher);
        ILogEnricher<T> WithProperty(ILogEventEnricher enricher);
    }
}