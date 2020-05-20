using System;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Sol;

namespace Solari.Callisto.Connector
{
    public class CallistoConnectionFactory : ICallistoConnectionFactory
    {
        public ICallistoConnection Make(CallistoConnectorOptions callistoOptions, ApplicationOptions appOptions)
        {
            return new CallistoConnectionBuilder()
                   .WithMongoClient(builder => builder.WithCallistoConnectionOptions(callistoOptions, appOptions.ApplicationName).Build())
                   .WithDataBaseName(callistoOptions.Database)
                   .Build();
        }

        public ICallistoConnection Make(Func<CallistoConnectionBuilder, CallistoConnection> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            return builder(new CallistoConnectionBuilder());
        }

        public ICallistoConnection Make(MongoClientSettings settings, string databaseName)
        {
            return new CallistoConnectionBuilder().WithMongoClient(builder => builder.WithMongoClientSettings(settings)
                                                                                     .Build())
                                                  .WithDataBaseName(databaseName)
                                                  .Build();
        }

        public ICallistoConnection Make(MongoClient client, string databaseName)
        {
            return new CallistoConnectionBuilder().WithMongoClient(client)
                                                  .WithDataBaseName(databaseName)
                                                  .Build();
        }
    }
}