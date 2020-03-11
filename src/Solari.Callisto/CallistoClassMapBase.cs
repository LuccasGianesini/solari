using System;
using MongoDB.Bson.Serialization;

namespace Solari.Callisto
{
    public abstract class CallistoClassMap : CallistoClassMapBase
    {
        protected CallistoClassMap(Type type) : base(type)
        {
        }

        public abstract BsonClassMap Configure();
    }

    public abstract class CallistoClassMap<T> : CallistoClassMapBase
    {
        protected CallistoClassMap() : base(typeof(T))
        {
        }
        public abstract BsonClassMap Configure();
    }
    public class CallistoClassMapBase
    {
        public CallistoClassMapBase(Type type)
        {
            BsonClassMap = new BsonClassMap(type);
            Type = type;
        }
        public BsonClassMap BsonClassMap { get; }
        public Type Type { get; private set; }
    }
}