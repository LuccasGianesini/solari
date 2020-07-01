using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace Solari.Callisto.Abstractions
{
    public class CallistoConnectorOptions
    {
        public string ConnectionString { get; set; }
        public string ConnectTimeout { get; set; }
        public string HeartbeatInterval { get; set; }
        public MassTransitOptions MassTransitStorageConfiguration { get; set; }
        public string HeartbeatTimeout { get; set; }
        /// <summary>
        ///     List of host;port
        /// </summary>
        public List<string> Servers { get; set; }
        public bool Ipv6 { get; set; }
        public string LocalThreshold { get; set; }
        public string MaxConnectionIdleTime { get; set; }
        public string MaxConnectionLifeTime { get; set; }
        public int MaxConnectionPoolSize { get; set; }
        public int MinConnectionPoolSize { get; set; }
        public string Database { get; set; }
        public bool RetryWrites { get; set; }
        public string WriteConcern { get; set; }
        public bool RetryReads { get; set; }
        public string ReadConcern { get; set; }
        public string ReadPreference { get; set; }
        public string ServerSelectionTimeout { get; set; }
        public string SocketTimeout { get; set; }
        public string WaitQueueTimeout { get; set; }
        public string ConnectionMode { get; set; }
        public string GuidRepresentation { get; set; }
        public string ConnectionStringScheme { get; set; }

        public WriteConcern GetWriteConcern()
        {
            if (string.IsNullOrEmpty(WriteConcern)) return MongoDB.Driver.WriteConcern.Acknowledged;
            return WriteConcern.ToLowerInvariant() switch
                   {
                       "acknowledged"   => MongoDB.Driver.WriteConcern.Acknowledged,
                       "unacknowledged" => MongoDB.Driver.WriteConcern.Unacknowledged,
                       "w1"             => MongoDB.Driver.WriteConcern.W1,
                       "w2"             => MongoDB.Driver.WriteConcern.W2,
                       "w3"             => MongoDB.Driver.WriteConcern.W3,
                       "wmajority"      => MongoDB.Driver.WriteConcern.WMajority,
                       _                => MongoDB.Driver.WriteConcern.Acknowledged
                   };
        }

        public ReadPreference GetReadPreference()
        {
            if (string.IsNullOrEmpty(ReadPreference)) return MongoDB.Driver.ReadPreference.Primary;
            return ReadPreference.ToLowerInvariant() switch
                   {
                       "primary"            => MongoDB.Driver.ReadPreference.Primary,
                       "nearest"            => MongoDB.Driver.ReadPreference.Nearest,
                       "primarypreffered"   => MongoDB.Driver.ReadPreference.PrimaryPreferred,
                       "secondary"          => MongoDB.Driver.ReadPreference.Secondary,
                       "secondarypreffered" => MongoDB.Driver.ReadPreference.SecondaryPreferred,
                       _                    => MongoDB.Driver.ReadPreference.Primary
                   };
        }


        public ReadConcern GetReadConcern()
        {
            if (string.IsNullOrEmpty(ReadConcern)) return MongoDB.Driver.ReadConcern.Default;
            return ReadConcern.ToLowerInvariant() switch
                   {
                       "available"    => MongoDB.Driver.ReadConcern.Available,
                       "default"      => MongoDB.Driver.ReadConcern.Default,
                       "linearizable" => MongoDB.Driver.ReadConcern.Linearizable,
                       "local"        => MongoDB.Driver.ReadConcern.Local,
                       "majority"     => MongoDB.Driver.ReadConcern.Majority,
                       "snapshot"     => MongoDB.Driver.ReadConcern.Snapshot,
                       _              => MongoDB.Driver.ReadConcern.Default
                   };
        }

        public ConnectionMode GetConnectionMode()
        {
            if (string.IsNullOrEmpty(ConnectionMode)) return MongoDB.Driver.ConnectionMode.Automatic;
            return ConnectionMode.ToLowerInvariant() switch
                   {
                       "automatic"   => MongoDB.Driver.ConnectionMode.Automatic,
                       "direct"      => MongoDB.Driver.ConnectionMode.Direct,
                       "standalone"  => MongoDB.Driver.ConnectionMode.Standalone,
                       "replicaset"  => MongoDB.Driver.ConnectionMode.ReplicaSet,
                       "shardrouter" => MongoDB.Driver.ConnectionMode.ShardRouter,
                       _             => MongoDB.Driver.ConnectionMode.Automatic
                   };
        }

        public GuidRepresentation GetGuidRepresentation()
        {
            if (string.IsNullOrEmpty(GuidRepresentation)) return MongoDB.Bson.GuidRepresentation.Standard;
            return GuidRepresentation.ToLowerInvariant() switch
                   {
                       "unspecified"  => MongoDB.Bson.GuidRepresentation.Unspecified,
                       "standard"     => MongoDB.Bson.GuidRepresentation.Standard,
                       "csharplegacy" => MongoDB.Bson.GuidRepresentation.CSharpLegacy,
                       "javalegacy"   => MongoDB.Bson.GuidRepresentation.JavaLegacy,
                       "pythonlegacy" => MongoDB.Bson.GuidRepresentation.PythonLegacy,
                       _              => MongoDB.Bson.GuidRepresentation.Standard
                   };
        }

        public ConnectionStringScheme GetConnectionStringScheme()
        {
            if (string.IsNullOrEmpty(ConnectionStringScheme)) return MongoDB.Driver.Core.Configuration.ConnectionStringScheme.MongoDB;
            return ConnectionStringScheme.ToLowerInvariant() switch
                   {
                       "mongodb"     => MongoDB.Driver.Core.Configuration.ConnectionStringScheme.MongoDB,
                       "mongodb+srv" => MongoDB.Driver.Core.Configuration.ConnectionStringScheme.MongoDBPlusSrv,
                       _             => MongoDB.Driver.Core.Configuration.ConnectionStringScheme.MongoDB
                   };
        }
    }
}
