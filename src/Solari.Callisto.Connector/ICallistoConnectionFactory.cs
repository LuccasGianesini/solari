using System;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Sol;

namespace Solari.Callisto.Connector
{
    public interface ICallistoConnectionFactory
    {
        ICallistoConnection Make(CallistoConnectorOptions callistoOptions, ApplicationOptions appOptions);
        ICallistoConnection Make(Func<CallistoConnectionBuilder, CallistoConnection> builder);
        ICallistoConnection Make(MongoClientSettings settings, string databaseName);
        ICallistoConnection Make(MongoClient client, string databaseName);
    }
}