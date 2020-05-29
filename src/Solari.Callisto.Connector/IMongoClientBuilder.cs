using System;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;

namespace Solari.Callisto.Connector
{
    public interface IMongoClientBuilder
    {
        IMongoClientBuilder WithCallistoConnectionOptions(CallistoConnectorOptions connectorOptions, string applicationName);
        IMongoClientBuilder WithMongoClientSettings(MongoClientSettings clientSettings);
        IMongoClientBuilder WithMongoClientSettings(Func<MongoDbClientSettingsBuilder, MongoClientSettings> builder);
        IMongoClientBuilder WithConnectionString(string connectionString);
        IMongoClientBuilder WithMongoUrl(MongoUrl mongoUrl);
        IMongoClientBuilder WithMongoUrl(Func<MongoUrlBuilder, MongoUrl> builder);
        IMongoClientBuilder WithMongoUrl(Func<MongoUrlBuilder, MongoUrl> builder, string url);

        /// <summary>
        ///     Build an <see cref="MongoClient" /> with precedence order as follows: ConnectionString -> MongoClientSettings -> MongoUrl.
        /// </summary>
        /// <returns></returns>
        MongoClient Build();

        /// <summary>
        /// Used to set GUID representation.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        IMongoClientBuilder WithConnectorOptions(CallistoConnectorOptions options);

        IMongoClientBuilder WithApplicationName(string appName);
    }
}