using System;
using MongoDB.Bson.Serialization;
using Solari.Callisto.Framework;

namespace Solari.Callisto
{
    public interface ICallistoClassMapper
    {
        ICallistoClassMapper AutoRegister(AppDomainClasses maps);
        ICallistoClassMapper AutoRegister<TClass>() where TClass : class;
        ICallistoClassMapper AutoRegister(Type type);
        ICallistoClassMapper RegisterClassMap<TClass>(Action<BsonClassMap<TClass>> action) where TClass : class;
        ICallistoClassMapper RegisterClassMap<T>(CallistoClassMap<T> mapper);
        ICallistoClassMapper RegisterClassMap(CallistoClassMap mapper);
    }
}