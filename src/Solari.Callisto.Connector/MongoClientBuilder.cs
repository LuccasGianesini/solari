using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Sol.Extensions;

namespace Solari.Callisto.Connector
{
    public class MongoClientBuilder : IMongoClientBuilder
    {
        private string _connectionString;
        private MongoClientSettings _mongoClientSettings;
        private MongoUrl _mongoUrl;
        private CallistoConnectorOptions _options;

        public IMongoClientBuilder WithCallistoConnectionOptions(CallistoConnectorOptions connectorOptions, string applicationName)
        {
            if (connectorOptions == null) throw new ArgumentNullException(nameof(connectorOptions));
            _mongoClientSettings = new MongoDbClientSettingsBuilder()
                                   .WithConnectTimeout(connectorOptions.ConnectTimeout.ToTimeSpan())
                                   .WithHeartbeatInterval(connectorOptions.HeartbeatInterval.ToTimeSpan())
                                   .WithHeartBeatTimeout(connectorOptions.HeartbeatTimeout.ToTimeSpan())
                                   .WithServers(connectorOptions.Servers.Select(server =>
                                   {
                                       string[] hostPort = server.Split(":");
                                       return new MongoServerAddress(hostPort[0], int.Parse(hostPort[1]));
                                   }))
                                   .WithIpv6(connectorOptions.Ipv6)
                                   .WithLocalThreshold(connectorOptions.LocalThreshold.ToTimeSpan())
                                   .WithMaxConnectionIdleTime(connectorOptions.MaxConnectionIdleTime.ToTimeSpan())
                                   .WithMaxConnectionLifeTime(connectorOptions.MaxConnectionLifeTime.ToTimeSpan())
                                   .WithMaxConnectionPoolSize(connectorOptions.MaxConnectionPoolSize)
                                   .WithMinConnectionPoolSize(connectorOptions.MinConnectionPoolSize)
                                   .WithRetryWrites(connectorOptions.RetryWrites)
                                   .WithWriteConcern(connectorOptions.GetWriteConcern())
                                   .WithRetryReads(connectorOptions.RetryReads)
                                   .WithReadConcern(connectorOptions.GetReadConcern())
                                   .WithReadPreference(connectorOptions.GetReadPreference())
                                   .WithServerSelectionTimeout(connectorOptions.ServerSelectionTimeout.ToTimeSpan())
                                   .WithSocketTimeout(connectorOptions.SocketTimeout.ToTimeSpan())
                                   .WithWaitQueueTimeout(connectorOptions.WaitQueueTimeout.ToTimeSpan())
                                   .WithConnectionMode(connectorOptions.GetConnectionMode())
                                   .WithGuidRepresentation(connectorOptions.GetGuidRepresentation())
                                   .WithConnectionStringScheme(connectorOptions.GetConnectionStringScheme())
                                   .WithApplicationName(applicationName)
                                   .Build();

            return this;
        }

        public IMongoClientBuilder WithMongoClientSettings(MongoClientSettings clientSettings)
        {
            _mongoClientSettings = clientSettings;
            return this;
        }

        public IMongoClientBuilder WithMongoClientSettings(Func<MongoDbClientSettingsBuilder, MongoClientSettings> builder)
        {
            _mongoClientSettings = builder(new MongoDbClientSettingsBuilder());
            return this;
        }

        public IMongoClientBuilder WithConnectionString(string connectionString)
        {
            _connectionString = connectionString;
            return this;
        }

        public IMongoClientBuilder WithMongoUrl(MongoUrl mongoUrl)
        {
            _mongoUrl = mongoUrl;
            return this;
        }

        public IMongoClientBuilder WithMongoUrl(Func<MongoUrlBuilder, MongoUrl> builder)
        {
            _mongoUrl = builder(new MongoUrlBuilder());
            return this;
        }

        public IMongoClientBuilder WithMongoUrl(Func<MongoUrlBuilder, MongoUrl> builder, string url)
        {
            _mongoUrl = builder(new MongoUrlBuilder(url));
            return this;
        }

        /// <summary>
        /// Used to set GUID representation.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public IMongoClientBuilder WithConnectorOptions(CallistoConnectorOptions options)
        {
            _options = options;
            return this;
        }

        /// <summary>
        ///     Build an <see cref="MongoClient" /> with precedence order as follows: ConnectionString -> MongoClientSettings -> MongoUrl.
        /// </summary>
        /// <returns></returns>
        public MongoClient Build()
        {
            if (CreateUsingConnectionString(out MongoClient mongoClient)) return mongoClient;

            return _mongoClientSettings != null ? new MongoClient(_mongoClientSettings) : new MongoClient(_mongoUrl);
        }

        private bool CreateUsingConnectionString(out MongoClient mongoClient)
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                MongoClientSettings settings = new MongoClient(_connectionString).Settings.Clone();
                settings.GuidRepresentation = _options?.GetGuidRepresentation() ?? GuidRepresentation.Standard;

                {
                    mongoClient = new MongoClient(settings);
                    return true;
                }
            }

            mongoClient = null;
            return false;
        }
    }
}