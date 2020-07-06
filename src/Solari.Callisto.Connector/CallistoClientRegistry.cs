using System.Collections.Generic;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Sol.Extensions;

namespace Solari.Callisto.Connector
{
    public class CallistoClientRegistry : ICallistoClientRegistry
    {
        private readonly Dictionary<string, ICallistoClient> _clients;

        private static readonly ICallistoClientRegistry _instance = new CallistoClientRegistry();

        // ReSharper disable once ConvertToAutoProperty
        public static ICallistoClientRegistry Instance => _instance;

        static CallistoClientRegistry()
        {

        }

        private CallistoClientRegistry()
        {
            _clients = new Dictionary<string, ICallistoClient>(5);
        }

        public void AddClient(string clientName, ICallistoClient client)
        {
            if (client is null)
                throw new CallistoException($"A null instance of a '{nameof(ICallistoClient)}' is not acceptable.");

            if (string.IsNullOrEmpty(clientName))
                throw new CallistoException("To add a client to the registry, you must provide a key.");

            if (!_clients.TryAdd(clientName.ToUpperInvariant(), client))
                throw new CallistoException($"The application was unable to add the client '{clientName}' into the registry.");
        }

        public ICallistoClient GetClient(string clientName)
        {
            string key = ValidateAndParseRegistryKey(clientName);

            if (!_clients.TryGetValue(key, out ICallistoClient client))
                throw new CallistoException($"The application was unable to retrieve the client '{clientName}' from the registry.");

            return client;
        }

        public void RemoveClient(string clientName)
        {
            string key = ValidateAndParseRegistryKey(clientName);
            if (!_clients.TryRemove(key, out ICallistoClient _))
                throw new CallistoException($"The application was unable to remove the client '{clientName}' from the registry.");
        }

        public void UpdateClient(string clientName, ICallistoClient client)
        {
            string key = ValidateAndParseRegistryKey(clientName);
            if (!_clients.TryUpdate(key, client))
                throw new CallistoException($"The application was unable to update the client '{clientName}.'");
        }

        private string ValidateAndParseRegistryKey(string clientName)
        {
            if (string.IsNullOrEmpty(clientName))
                throw new CallistoException("The application is unable to read the registry without a valid key.");

            string key = clientName.ToUpperInvariant();
            if (!_clients.ContainsKey(key))
                throw new CallistoException($"The registry does not contains the given key: {clientName}");

            return key;
        }
    }
}
