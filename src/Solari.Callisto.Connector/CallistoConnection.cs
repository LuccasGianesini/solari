using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Clusters;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Exceptions;

namespace Solari.Callisto.Connector
{
    public sealed class CallistoConnection : ICallistoConnection
    {
        private readonly Dictionary<string, IMongoClient> _client;
        private IMongoDatabase _database;

        public CallistoConnection()
        {
            _client = new Dictionary<string, IMongoClient>(1);
        }

        public IMongoDatabase GetDataBase() { return GetClient().GetDatabase(DataBaseName); }

        public ICallistoConnection AddClient(IMongoClient mongoClient)
        {
            if (mongoClient == null) throw new CallistoException($"Null {nameof(IMongoClient)} is not accepted in the connection dictionary");

            _client.TryAdd(CallistoConstants.CallistoConnectorCacheKey, mongoClient);
            CallistoLogger.ConnectionLogger.AddingMongoClient();
            return this;
        }

        public ICallistoConnection AddDataBase(string dataBaseName)
        {
            if (string.IsNullOrEmpty(dataBaseName)) throw new ArgumentException("Value cannot be null or empty.", nameof(dataBaseName));
            DataBaseName = dataBaseName;
            return this;
        }

        public IMongoClient GetClient() { return _client.TryGetValue(CallistoConstants.CallistoConnectorCacheKey, out IMongoClient client) ? client : default; }

        public string DataBaseName { get; private set; }

        public void UpdateClient(IMongoClient client)
        {
            _client[CallistoConstants.CallistoConnectorCacheKey] = client ?? throw new ArgumentNullException(nameof(client));
            CallistoLogger.ConnectionLogger.UpdatingMongoClient();
            IsConnected().GetAwaiter().GetResult();
        }

        public void ChangeDatabase(string dataBaseName)
        {
            if (string.IsNullOrEmpty(dataBaseName)) throw new ArgumentException("Value cannot be null or empty.", nameof(dataBaseName));
            _database = GetClient().GetDatabase(dataBaseName);
            DataBaseName = dataBaseName;
            CallistoLogger.ConnectionLogger.ChangingDatabase(dataBaseName);
        }

        public async Task<CallistoConnectionCheck> IsConnected(CancellationToken? cancellationToken = null)
        {
            try
            {
                var ping = await GetDataBase().RunCommandAsync<BsonDocument>(new BsonDocument {{"ping", 1}}, default, cancellationToken
                                                                                                                   ?? CancellationToken.None);
                if (ping.Contains("ok") &&
                    (ping["ok"].IsDouble && (int) ping["ok"].AsDouble == 1 ||
                     ping["ok"].IsInt32 && ping["ok"].AsInt32 == 1))
                {
                    ClusterState state = GetClient().Cluster.Description.State;
                    CallistoLogger.ConnectionLogger.ConnectionStatus(state.ToString(), ping.ToString());
                    return new CallistoConnectionCheck(state, ping.ToString());
                }

                CallistoLogger.ConnectionLogger.ConnectionStatus(ClusterState.Disconnected.ToString(), ping.ToString());
                return new CallistoConnectionCheck(ClusterState.Disconnected, ping.ToString());
            }
            catch (Exception e)
            {
                CallistoLogger.ConnectionLogger.ConnectionStatus(ClusterState.Disconnected.ToString(), e.Message);
                return new CallistoConnectionCheck(ClusterState.Disconnected, e.Message);
            }
        }
    }
}
