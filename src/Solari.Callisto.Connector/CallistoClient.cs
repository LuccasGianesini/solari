using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Clusters;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts;

namespace Solari.Callisto.Connector
{
    public class CallistoClient : ICallistoClient
    {
        public string ConnectionString { get; }
        public IMongoClient MongoClient { get; }

        public CallistoClient(IMongoClient mongoClient)
        {
            MongoClient = mongoClient;
            ConnectionString = mongoClient.Settings.ToString();
        }

        public async Task<CallistoConnectionCheck> IsConnected(string database = "admin", CancellationToken? cancellationToken = null)
        {
            try
            {
                var ping = await MongoClient.GetDatabase(database)
                                            .RunCommandAsync<BsonDocument>(new BsonDocument {{"ping", 1}},
                                                                           default,
                                                                           cancellationToken ?? CancellationToken.None);
                if (ping.Contains("ok") &&
                    (ping["ok"].IsDouble && (int) ping["ok"].AsDouble == 1 ||
                     ping["ok"].IsInt32 && ping["ok"].AsInt32 == 1))
                {
                    ClusterState state = MongoClient.Cluster.Description.State;
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
