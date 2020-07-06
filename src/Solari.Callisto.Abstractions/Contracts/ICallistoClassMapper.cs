using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization;

namespace Solari.Callisto.Abstractions.Contracts
{
    public interface ICallistoClassMapper
    {
        ICallistoClassMapper RegisterClassMaps(IEnumerable<Type> appDomain);
        BsonClassMap CreateBsonClassMap(Type type, Action<BsonClassMap> mapperAction);
        void RegisterClassMap(Type type, BsonClassMap bsonClassMap);
    }
}
