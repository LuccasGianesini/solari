using System;
using MongoDB.Bson.Serialization.Conventions;

namespace Solari.Callisto.Abstractions.Contracts
{
    public interface ICallistoConventionRegistry
    {
        ConventionPack ConventionPack { get; }
        ICallistoConventionRegistry AddDefaultConventions();
        ICallistoConventionRegistry ConfigureConventionPack(Action<ConventionPack> configurationAction);
        void RegisterConventionPack();
        void RegisterConventionPack(string name, Func<Type, bool> filter);
    }
}
