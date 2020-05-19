using System;
using System.Linq;
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

        public MongoClientBuilder WithCallistoConnectionOptions(CallistoConnectorOptions connectorOptions, string applicationName)
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
                                   // .WithWaitQueueSize(connectorOptions.WaitQueueSize)
                                   .WithWaitQueueTimeout(connectorOptions.WaitQueueTimeout.ToTimeSpan())
                                   .WithConnectionMode(connectorOptions.GetConnectionMode())
                                   .WithGuidRepresentation(connectorOptions.GetGuidRepresentation())
                                   .WithConnectionStringScheme(connectorOptions.GetConnectionStringScheme())
                                   .WithApplicationName(applicationName)
                                   .Build();

            return this;
        }

        public MongoClientBuilder WithMongoClientSettings(MongoClientSettings clientSettings)
        {
            _mongoClientSettings = clientSettings;
            return this;
        }

        public MongoClientBuilder WithMongoClientSettings(Func<MongoDbClientSettingsBuilder, MongoClientSettings> builder)
        {
            _mongoClientSettings = builder(new MongoDbClientSettingsBuilder());
            return this;
        }

        public MongoClientBuilder WithConnectionString(string connectionString)
        {
            _connectionString = connectionString;
            return this;
        }

        public MongoClientBuilder WithMongoUrl(MongoUrl mongoUrl)
        {
            _mongoUrl = mongoUrl;
            return this;
        }

        public MongoClientBuilder WithMongoUrl(Func<MongoUrlBuilder, MongoUrl> builder)
        {
            _mongoUrl = builder(new MongoUrlBuilder());
            return this;
        }

        public MongoClientBuilder WithMongoUrl(Func<MongoUrlBuilder, MongoUrl> builder, string url)
        {
            _mongoUrl = builder(new MongoUrlBuilder(url));
            return this;
        }


        /// <summary>
        ///     Build an <see cref="MongoClient" /> with precedence order as follows: ConnectionString -> MongoClientSettings -> MongoUrl.
        /// </summary>
        /// <returns></returns>
        public MongoClient Build()
        {
            if (!string.IsNullOrEmpty(_connectionString)) return new MongoClient(_connectionString);

            return _mongoClientSettings != null ? new MongoClient(_mongoClientSettings) : new MongoClient(_mongoUrl);
        }
    }
}