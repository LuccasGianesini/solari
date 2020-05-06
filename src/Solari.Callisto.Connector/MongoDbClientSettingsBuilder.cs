using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver.Encryption;

// ReSharper disable IdentifierTypo

namespace Solari.Callisto.Connector
{
    public class MongoDbClientSettingsBuilder : IMongoDbClientSettingsBuilder
    {
        private bool _allowInsecureTls;
        private string _applicationName;
        private AutoEncryptionOptions _autoEncryptionOptions;
        private Action<ClusterBuilder> _clusterBuilder;
        private IReadOnlyList<CompressorConfiguration> _compressors = new CompressorConfiguration[0];
        private ConnectionMode _connectionMode = ConnectionMode.Automatic;
        private TimeSpan _connectTimeout = MongoDefaults.ConnectTimeout;
        private GuidRepresentation _guidRepresentation = GuidRepresentation.Standard;
        private TimeSpan _heartBeatInterval = ServerSettings.DefaultHeartbeatInterval;
        private TimeSpan _heartBeatTimeout = ServerSettings.DefaultHeartbeatTimeout;
        private bool _ipv6;
        private TimeSpan _localThreshold;
        private TimeSpan _maxConnectionIdleTime = MongoDefaults.MaxConnectionIdleTime;
        private TimeSpan _maxConnectionLifeTime = MongoDefaults.MaxConnectionLifeTime;
        private int _maxConnectionPoolSize = MongoDefaults.MaxConnectionPoolSize;
        private int _minConnectionPoolSize = MongoDefaults.MinConnectionPoolSize;
        private MongoCredential _mongoCredential;
        private ReadConcern _readConcern = ReadConcern.Default;
        private ReadPreference _readPreference = ReadPreference.Primary;
        private string _replicaSetName;
        private bool _retryReads = true;
        private bool _retryWrites = true;
        private ConnectionStringScheme _scheme = ConnectionStringScheme.MongoDB;
        private string _sdamLogFileName;
        private readonly List<MongoServerAddress> _serverAddresses = new List<MongoServerAddress>();
        private TimeSpan _serverSelectionTimeout = MongoDefaults.ServerSelectionTimeout;
        private TimeSpan _socketTimeout = MongoDefaults.SocketTimeout;
        private SslSettings _sslSettings;
        private bool _useTls;
        private UTF8Encoding _utf8ReadEncoding = new UTF8Encoding();
        private UTF8Encoding _utf8WriteEncoding = new UTF8Encoding();
        private TimeSpan _waitQueueTimeout = MongoDefaults.WaitQueueTimeout;

        private WriteConcern _writeConcern = WriteConcern.Acknowledged;
        // private int _waitQueueSize = MongoDefaults.ComputedWaitQueueSize;


        /// <summary>
        ///     The timeout for the wait queue. If TimeSpan.MinValue mongo's default value will be used.
        /// </summary>
        /// <param name="waitQueueTimeout">The timeout</param>
        /// <returns></returns>
        public MongoDbClientSettingsBuilder WithWaitQueueTimeout(TimeSpan waitQueueTimeout)
        {
            if (waitQueueTimeout == TimeSpan.MinValue) return this;
            _waitQueueTimeout = waitQueueTimeout;
            return this;
        }

        // /// <summary>
        // /// The wait queue size. If <=0 mongo's default value will be used. 
        // /// </summary>
        // /// <param name="waitQueueSize">The wait queue size</param>
        // /// <returns></returns>
        // public MongoDbClientSettingsBuilder WithWaitQueueSize(int waitQueueSize)
        // {
        //     if (waitQueueSize <= 0) return this;
        //     _waitQueueSize = waitQueueSize;
        //     return this;
        // }

        public MongoDbClientSettingsBuilder WithTls(bool useTls)
        {
            _useTls = useTls;
            return this;
        }

        /// <summary>
        ///     The server selection timeout. If TimeSpan.MinValue mongo's default value will be used.
        /// </summary>
        /// <param name="serverSelectionTimeout">The timeout</param>
        /// <returns></returns>
        public MongoDbClientSettingsBuilder WithServerSelectionTimeout(TimeSpan serverSelectionTimeout)
        {
            if (serverSelectionTimeout == TimeSpan.MinValue) return this;
            _serverSelectionTimeout = serverSelectionTimeout;
            return this;
        }

        /// <summary>
        ///     The server selection timeout. If TimeSpan.MinValue mongo's default value will be used.
        /// </summary>
        /// <param name="socketTimeout">The timeout</param>
        /// <returns></returns>
        public MongoDbClientSettingsBuilder WithSocketTimeout(TimeSpan socketTimeout)
        {
            if (socketTimeout == TimeSpan.MinValue) return this;
            _socketTimeout = socketTimeout;
            return this;
        }

        public MongoDbClientSettingsBuilder WithSdamLogFileName(string sdamLogFileName)
        {
            _sdamLogFileName = sdamLogFileName;
            return this;
        }

        public MongoDbClientSettingsBuilder WithConnectionStringScheme(ConnectionStringScheme scheme)
        {
            _scheme = scheme;
            return this;
        }

        public MongoDbClientSettingsBuilder WithRetryWrites(bool retryWrites)
        {
            _retryWrites = retryWrites;
            return this;
        }

        public MongoDbClientSettingsBuilder WithRetryReads(bool retryReads)
        {
            _retryReads = retryReads;
            return this;
        }

        public MongoDbClientSettingsBuilder WithReplicaSetName(string replicaName)
        {
            _replicaSetName = replicaName;
            return this;
        }

        /// <summary>
        ///     The maximum pool size for the connection. If <=0  mongo's default value will be used. 
        /// </summary>
        /// <param name="maxConnectionPoolSize">The maximum connection pool size</param>
        /// <returns></returns>
        public MongoDbClientSettingsBuilder WithMaxConnectionPoolSize(int maxConnectionPoolSize)
        {
            if (maxConnectionPoolSize <= 0) return this;
            _maxConnectionPoolSize = maxConnectionPoolSize;
            return this;
        }

        /// <summary>
        ///     The minimum pool size for the connection. If <=0  mongo's default value will be used. 
        /// </summary>
        /// <param name="minConnectionPoolSize">The minimum connection pool size</param>
        /// <returns></returns>
        public MongoDbClientSettingsBuilder WithMinConnectionPoolSize(int minConnectionPoolSize)
        {
            if (minConnectionPoolSize <= 0) return this;
            _minConnectionPoolSize = minConnectionPoolSize;
            return this;
        }

        /// <summary>
        ///     The maximum lifetime of the connection. If <=0  mongo's default value will be used. 
        /// </summary>
        /// <param name="maxConnectionLifeTime">The maximum lifetime of the connection size</param>
        /// <returns></returns>
        public MongoDbClientSettingsBuilder WithMaxConnectionLifeTime(TimeSpan maxConnectionLifeTime)
        {
            if (maxConnectionLifeTime == TimeSpan.MinValue) return this;
            _maxConnectionLifeTime = maxConnectionLifeTime;
            return this;
        }

        /// <summary>
        ///     The maximum idle time of the connection. If <=0  mongo's default value will be used. 
        /// </summary>
        /// <param name="maxConnectionIdleTime"></param>
        /// <returns></returns>
        public MongoDbClientSettingsBuilder WithMaxConnectionIdleTime(TimeSpan maxConnectionIdleTime)
        {
            if (maxConnectionIdleTime == TimeSpan.MinValue) return this;
            _maxConnectionIdleTime = maxConnectionIdleTime;
            return this;
        }

        /// <summary>
        ///     The local threshold. If <=0  mongo's default value will be used. 
        /// </summary>
        /// <param name="localThreshold"></param>
        /// <returns></returns>
        public MongoDbClientSettingsBuilder WithLocalThreshold(TimeSpan localThreshold)
        {
            if (localThreshold == TimeSpan.MinValue) return this;
            _localThreshold = localThreshold;
            return this;
        }

        public MongoDbClientSettingsBuilder WithIpv6(bool ipv6)
        {
            _ipv6 = ipv6;
            return this;
        }

        /// <summary>
        ///     The heartbeat interval. If <=0  mongo's default value will be used. 
        /// </summary>
        /// <param name="heartbeatInterval"></param>
        /// <returns></returns>
        public MongoDbClientSettingsBuilder WithHeartbeatInterval(TimeSpan heartbeatInterval)
        {
            if (heartbeatInterval == TimeSpan.MinValue) return this;
            _heartBeatInterval = heartbeatInterval;
            return this;
        }

        /// <summary>
        ///     The heartbeat timeout. If <=0  mongo's default value will be used. 
        /// </summary>
        /// <param name="heartbeatTimeout"></param>
        /// <returns></returns>
        public MongoDbClientSettingsBuilder WithHeartBeatTimeout(TimeSpan heartbeatTimeout)
        {
            if (heartbeatTimeout == TimeSpan.MinValue) return this;
            _heartBeatTimeout = heartbeatTimeout;
            return this;
        }

        /// <summary>
        ///     The connect timeout. If <=0  mongo's default value will be used. 
        /// </summary>
        /// <param name="connectTimeout">The maximum connection pool size</param>
        /// <returns></returns>
        public MongoDbClientSettingsBuilder WithConnectTimeout(TimeSpan connectTimeout)
        {
            if (connectTimeout == TimeSpan.MinValue) return this;
            _connectTimeout = connectTimeout;
            return this;
        }

        public MongoDbClientSettingsBuilder WithCompressors(IReadOnlyList<CompressorConfiguration> compressors)
        {
            _compressors = compressors;
            return this;
        }

        public MongoDbClientSettingsBuilder WithAutoEncryptionOptions(AutoEncryptionOptions autoEncryptionOptions)
        {
            _autoEncryptionOptions = autoEncryptionOptions;
            return this;
        }

        public MongoDbClientSettingsBuilder WithApplicationName(string applicationName)
        {
            _applicationName = applicationName;
            return this;
        }

        public MongoDbClientSettingsBuilder WithInsecureTls(bool allowInsecureTls)
        {
            _allowInsecureTls = allowInsecureTls;
            return this;
        }

        public MongoDbClientSettingsBuilder WithClusterBuilder(Action<ClusterBuilder> clusterBuilder)
        {
            _clusterBuilder = clusterBuilder;
            return this;
        }


        public MongoDbClientSettingsBuilder WithConnectionMode(ConnectionMode connectionMode)
        {
            _connectionMode = connectionMode;
            return this;
        }

        public MongoDbClientSettingsBuilder WithCredentials(MongoCredential credential)
        {
            _mongoCredential = credential;
            return this;
        }

        public MongoDbClientSettingsBuilder WithGuidRepresentation(GuidRepresentation guidRepresentation)
        {
            _guidRepresentation = guidRepresentation;
            return this;
        }

        public MongoDbClientSettingsBuilder WithReadConcern(ReadConcern readConcern)
        {
            _readConcern = readConcern;
            return this;
        }

        public MongoDbClientSettingsBuilder WithReadEncoding(UTF8Encoding readEncoding)
        {
            _utf8ReadEncoding = readEncoding;
            return this;
        }

        public MongoDbClientSettingsBuilder WithReadPreference(ReadPreference readPreference)
        {
            _readPreference = readPreference;
            return this;
        }


        public MongoDbClientSettingsBuilder WithServers(string host, int port = 27017)
        {
            if (string.IsNullOrEmpty(host)) throw new ArgumentException("Value cannot be null or empty.", nameof(host));
            _serverAddresses.Add(new MongoServerAddress(host, port));
            return this;
        }

        public MongoDbClientSettingsBuilder WithServers(IEnumerable<MongoServerAddress> servers)
        {
            IEnumerable<MongoServerAddress> mongoServerAddresses = servers.ToList();
            if (!mongoServerAddresses.Any()) return this;
            _serverAddresses.AddRange(mongoServerAddresses);
            return this;
        }

        public MongoDbClientSettingsBuilder WithSslSettings(SslSettings sslSettings)
        {
            _sslSettings = sslSettings;
            return this;
        }

        public MongoDbClientSettingsBuilder WithWriteConcern(WriteConcern writeConcern)
        {
            _writeConcern = writeConcern;
            return this;
        }

        public MongoDbClientSettingsBuilder WithWriteEncoding(UTF8Encoding writeEncoding)
        {
            _utf8WriteEncoding = writeEncoding;
            return this;
        }


        public MongoClientSettings Build()
        {
            return new MongoClientSettings
            {
                Compressors = _compressors,
                Credential = _mongoCredential,
                Scheme = _scheme,
                Servers = _serverAddresses,
                ApplicationName = _applicationName,
                ClusterConfigurator = _clusterBuilder,
                ConnectionMode = _connectionMode,
                ConnectTimeout = _connectTimeout,
                GuidRepresentation = _guidRepresentation,
                HeartbeatInterval = _heartBeatInterval,
                HeartbeatTimeout = _heartBeatTimeout,
                LocalThreshold = _localThreshold,
                IPv6 = _ipv6,
                ReadConcern = _readConcern,
                ReadEncoding = _utf8ReadEncoding,
                ReadPreference = _readPreference,
                RetryReads = _retryReads,
                RetryWrites = _retryWrites,
                SocketTimeout = _socketTimeout,
                SslSettings = _sslSettings,
                UseTls = _useTls,
                WriteConcern = _writeConcern,
                WriteEncoding = _utf8WriteEncoding,
                AllowInsecureTls = _allowInsecureTls,
                AutoEncryptionOptions = _autoEncryptionOptions,
                ReplicaSetName = _replicaSetName,
                SdamLogFilename = _sdamLogFileName,
                ServerSelectionTimeout = _serverSelectionTimeout,
                WaitQueueTimeout = _waitQueueTimeout,
                // WaitQueueSize = _waitQueueSize,
                MaxConnectionIdleTime = _maxConnectionIdleTime,
                MaxConnectionLifeTime = _maxConnectionLifeTime,
                MaxConnectionPoolSize = _maxConnectionPoolSize,
                MinConnectionPoolSize = _minConnectionPoolSize
            };
        }
    }
}