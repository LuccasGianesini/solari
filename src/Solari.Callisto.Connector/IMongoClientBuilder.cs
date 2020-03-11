using System;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;

namespace Solari.Callisto.Connector
{
    public interface IMongoClientBuilder
    {
        MongoClientBuilder WithCallistoConnectionOptions(CallistoConnectorOptions connectorOptions, string applicationName);
        MongoClientBuilder WithMongoClientSettings(MongoClientSettings clientSettings);
        MongoClientBuilder WithMongoClientSettings(Func<MongoDbClientSettingsBuilder, MongoClientSettings> builder);
        MongoClientBuilder WithConnectionString(string connectionString);
        MongoClientBuilder WithMongoUrl(MongoUrl mongoUrl);
        MongoClientBuilder WithMongoUrl(Func<MongoUrlBuilder, MongoUrl> builder);
        MongoClientBuilder WithMongoUrl(Func<MongoUrlBuilder, MongoUrl> builder, string url);

        /// <summary>
        /// Build an <see cref="MongoClient"/> with precedence order as follows: ConnectionString -> MongoClientSettings -> MongoUrl.
        /// </summary>
        /// <returns></returns>
        MongoClient Build();
    }
}