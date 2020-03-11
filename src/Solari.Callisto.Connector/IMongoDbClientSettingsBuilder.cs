using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver.Encryption;

namespace Solari.Callisto.Connector
{
    public interface IMongoDbClientSettingsBuilder
    {
        /// <summary>
        /// The timeout for the wait queue. If TimeSpan.MinValue mongo's default value will be used. 
        /// </summary>
        /// <param name="waitQueueTimeout">The timeout</param>
        /// <returns></returns>
        MongoDbClientSettingsBuilder WithWaitQueueTimeout(TimeSpan waitQueueTimeout);

        // /// <summary>
        // /// The wait queue size. If <=0 mongo's default value will be used. 
        // /// </summary>
        // /// <param name="waitQueueSize">The wait queue size</param>
        // /// <returns></returns>
        // MongoDbClientSettingsBuilder WithWaitQueueSize(int waitQueueSize);

        MongoDbClientSettingsBuilder WithTls(bool useTls);

        /// <summary>
        /// The server selection timeout. If TimeSpan.MinValue mongo's default value will be used.
        /// </summary>
        /// <param name="serverSelectionTimeout">The timeout</param>
        /// <returns></returns>
        MongoDbClientSettingsBuilder WithServerSelectionTimeout(TimeSpan serverSelectionTimeout);

        /// <summary>
        /// The server selection timeout. If TimeSpan.MinValue mongo's default value will be used.
        /// </summary>
        /// <param name="socketTimeout">The timeout</param>
        /// <returns></returns>
        MongoDbClientSettingsBuilder WithSocketTimeout(TimeSpan socketTimeout);

        MongoDbClientSettingsBuilder WithSdamLogFileName(string sdamLogFileName);
        MongoDbClientSettingsBuilder WithConnectionStringScheme(ConnectionStringScheme scheme);
        MongoDbClientSettingsBuilder WithRetryWrites(bool retryWrites);
        MongoDbClientSettingsBuilder WithRetryReads(bool retryReads);
        MongoDbClientSettingsBuilder WithReplicaSetName(string replicaName);

        /// <summary>
        /// The maximum pool size for the connection. If <=0  mongo's default value will be used. 
        /// </summary>
        /// <param name="maxConnectionPoolSize">The maximum connection pool size</param>
        /// <returns></returns>
        MongoDbClientSettingsBuilder WithMaxConnectionPoolSize(int maxConnectionPoolSize);

        /// <summary>
        /// The minimum pool size for the connection. If <=0  mongo's default value will be used. 
        /// </summary>
        /// <param name="minConnectionPoolSize">The minimum connection pool size</param>
        /// <returns></returns>
        MongoDbClientSettingsBuilder WithMinConnectionPoolSize(int minConnectionPoolSize);

        /// <summary>
        /// The maximum lifetime of the connection. If <=0  mongo's default value will be used. 
        /// </summary>
        /// <param name="maxConnectionLifeTime">The maximum lifetime of the connection size</param>
        /// <returns></returns>
        MongoDbClientSettingsBuilder WithMaxConnectionLifeTime(TimeSpan maxConnectionLifeTime);

        /// <summary>
        /// The maximum idle time of the connection. If <=0  mongo's default value will be used. 
        /// </summary>
        /// <param name="maxConnectionIdleTime"></param>
        /// <returns></returns>
        MongoDbClientSettingsBuilder WithMaxConnectionIdleTime(TimeSpan maxConnectionIdleTime);

        /// <summary>
        /// The local threshold. If <=0  mongo's default value will be used. 
        /// </summary>
        /// <param name="localThreshold"></param>
        /// <returns></returns>
        MongoDbClientSettingsBuilder WithLocalThreshold(TimeSpan localThreshold);

        MongoDbClientSettingsBuilder WithIpv6(bool ipv6);

        /// <summary>
        /// The heartbeat interval. If <=0  mongo's default value will be used. 
        /// </summary>
        /// <param name="heartbeatInterval"></param>
        /// <returns></returns>
        MongoDbClientSettingsBuilder WithHeartbeatInterval(TimeSpan heartbeatInterval);

        /// <summary>
        /// The heartbeat timeout. If <=0  mongo's default value will be used. 
        /// </summary>
        /// <param name="heartbeatTimeout"></param>
        /// <returns></returns>
        MongoDbClientSettingsBuilder WithHeartBeatTimeout(TimeSpan heartbeatTimeout);

        /// <summary>
        /// The connect timeout. If <=0  mongo's default value will be used. 
        /// </summary>
        /// <param name="connectTimeout">The maximum connection pool size</param>
        /// <returns></returns>
        MongoDbClientSettingsBuilder WithConnectTimeout(TimeSpan connectTimeout);

        MongoDbClientSettingsBuilder WithCompressors(IReadOnlyList<CompressorConfiguration> compressors);
        MongoDbClientSettingsBuilder WithAutoEncryptionOptions(AutoEncryptionOptions autoEncryptionOptions);
        MongoDbClientSettingsBuilder WithApplicationName(string applicationName);
        MongoDbClientSettingsBuilder WithInsecureTls(bool allowInsecureTls);
        MongoDbClientSettingsBuilder WithClusterBuilder(Action<ClusterBuilder> clusterBuilder);
        MongoDbClientSettingsBuilder WithConnectionMode(ConnectionMode connectionMode);
        MongoDbClientSettingsBuilder WithCredentials(MongoCredential credential);
        MongoDbClientSettingsBuilder WithGuidRepresentation(GuidRepresentation guidRepresentation);
        MongoDbClientSettingsBuilder WithReadConcern(ReadConcern readConcern);
        MongoDbClientSettingsBuilder WithReadEncoding(UTF8Encoding readEncoding);
        MongoDbClientSettingsBuilder WithReadPreference(ReadPreference readPreference);
        MongoDbClientSettingsBuilder WithServers(string host, int port = 27017);
        MongoDbClientSettingsBuilder WithServers(IEnumerable<MongoServerAddress> servers);
        MongoDbClientSettingsBuilder WithSslSettings(SslSettings sslSettings);
        MongoDbClientSettingsBuilder WithWriteConcern(WriteConcern writeConcern);
        MongoDbClientSettingsBuilder WithWriteEncoding(UTF8Encoding writeEncoding);
        MongoClientSettings Build();
    }
}