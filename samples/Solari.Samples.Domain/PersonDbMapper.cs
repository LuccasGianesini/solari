using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using Solari.Callisto;
using Solari.Callisto.Serializers;

namespace Solari.Samples.Domain
{
    public static class PersonDbMapper
    {
        public static BsonClassMap<Person.Person> PersonMap
            => BsonClassMap.RegisterClassMap<Person.Person>(map =>
            {
                map.AutoMap();
                map.UnmapMember(a => a.PendingUpdates);
                map.MapCallistoId();
                map.MapCallistoDatetimeOffset(a => a.CreatedAt);
                map.MapMember(a => a.Address)
                   .SetDefaultValue("");
                map.SetIsRootClass(true);

            });
    }
}
