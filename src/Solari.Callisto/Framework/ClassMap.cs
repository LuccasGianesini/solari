using System;
using System.Linq.Expressions;
using MongoDB.Bson.Serialization;

namespace Solari.Callisto.Framework
{
    internal static class ClassMap
    {
        internal static BsonClassMap Create(Type type)
        {
            var map = new BsonClassMap(type);
            map.AutoMap();
            return map;
        }
    }
}