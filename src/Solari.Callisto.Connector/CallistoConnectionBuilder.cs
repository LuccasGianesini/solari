using System;
using MongoDB.Driver;

namespace Solari.Callisto.Connector
{
    public class CallistoConnectionBuilder
    {
        private string _dataBaseName;

        private IMongoClient _mongoClient;


        public CallistoConnectionBuilder WithMongoClient(Func<MongoClientBuilder, MongoClient> builder)
        {
            _mongoClient = builder(new MongoClientBuilder());
            return this;
        }

        public CallistoConnectionBuilder WithMongoClient(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
            return this;
        }

        public CallistoConnectionBuilder WithDataBaseName(string dataBaseName)
        {
            _dataBaseName = dataBaseName;
            return this;
        }

        public ICallistoConnection Build()
        {
            if(_mongoClient == null) throw new ArgumentException("No MongoClient created. Make sure build the client beforehand");
            if(string.IsNullOrEmpty(_dataBaseName)) throw new ArgumentException("No database name supplied.");
            return new CallistoConnection().AddClient(_mongoClient).AddDataBase(_dataBaseName);
        }
    }
}