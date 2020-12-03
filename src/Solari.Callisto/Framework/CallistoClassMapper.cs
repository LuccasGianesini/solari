using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Sol;
using Solari.Sol.Abstractions;

namespace Solari.Callisto.Framework
{
    public class CallistoClassMapper : ICallistoClassMapper
    {
        public ICallistoClassMapper RegisterClassMaps(IEnumerable<Type> appDomain)
        {
            foreach (Type type in appDomain.Where(a => !BsonClassMap.IsClassMapRegistered(a)))
            {
                RegisterClassMap(type, CreateBsonClassMap(type, map => map.AutoMap()));
            }

            return this;
        }

        public void RegisterClassMap(Type type, BsonClassMap bsonClassMap)
        {
            if (BsonClassMap.IsClassMapRegistered(type)) return;
            BsonClassMap.RegisterClassMap(bsonClassMap);
        }

        public BsonClassMap CreateBsonClassMap(Type type, Action<BsonClassMap> mapperAction)
        {
            Check.ThrowIfNull(mapperAction, nameof(Action<BsonClassMap>), new CallistoException("Cannot invoke a null 'mapperAction', please provide valid action."));
            var map = new BsonClassMap(type);
            mapperAction(map);
            return map;
        }
    }
}
