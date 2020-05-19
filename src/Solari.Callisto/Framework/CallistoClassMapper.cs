using System;
using System.Linq;
using MongoDB.Bson.Serialization;
using Solari.Callisto.Abstractions;

namespace Solari.Callisto.Framework
{
    public sealed class CallistoClassMapper : ICallistoClassMapper
    {
        public ICallistoClassMapper AutoRegister(AppDomainClasses maps)
        {
            foreach (Type type in maps.DocumentRoots.Where(type => !BsonClassMap.IsClassMapRegistered(type))) RegisterClassMap(type, ClassMap.Create(type));

            foreach (Type type in maps.DocumentNodes.Where(type => !BsonClassMap.IsClassMapRegistered(type))) RegisterClassMap(type, ClassMap.Create(type));

            return this;
        }

        public ICallistoClassMapper AutoRegister<TClass>() where TClass : class
        {
            RegisterClassMap(typeof(TClass), ClassMap.Create(typeof(TClass)));

            return this;
        }

        public ICallistoClassMapper AutoRegister(Type type)
        {
            RegisterClassMap(type, ClassMap.Create(type));

            return this;
        }


        public ICallistoClassMapper RegisterClassMap<TClass>(Action<BsonClassMap<TClass>> action) where TClass : class
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(TClass))) BsonClassMap.RegisterClassMap(action);
            return this;
        }

        public ICallistoClassMapper RegisterClassMap<T>(CallistoClassMap<T> mapper)
        {
            RegisterClassMap(mapper.Type, mapper.Configure());
            return this;
        }

        public ICallistoClassMapper RegisterClassMap(CallistoClassMap mapper)
        {
            RegisterClassMap(mapper.Type, mapper.Configure());
            return this;
        }

        private static void RegisterClassMap(Type type, BsonClassMap bsonClassMap)
        {
            if (BsonClassMap.IsClassMapRegistered(type)) return;
            BsonClassMap.RegisterClassMap(bsonClassMap);
            CallistoLogger.ClassMapsLogger.RegisteredClassMap(type.FullName);
        }
    }
}