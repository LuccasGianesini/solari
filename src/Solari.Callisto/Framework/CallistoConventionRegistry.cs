using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Callisto.Conventions;

namespace Solari.Callisto.Framework
{
    public class CallistoConventionRegistry : ICallistoConventionRegistry
    {
        public ConventionPack ConventionPack { get; }
        private readonly Func<Type, bool> _defaultFilter;
        private readonly string _defaultName = "solari-callisto-default-conventions";

        public CallistoConventionRegistry()
        {
            ConventionPack = new ConventionPack();
            _defaultFilter = CallistoTypeSelector.IsCallistoDocument;
        }

        public ICallistoConventionRegistry AddDefaultConventions()
        {
            ConventionPack.Add(new ReadWriteMemberFinderConvention());
            ConventionPack.Add(new CallistoIgnoreExtraElementsConvention());
            ConventionPack.Add(new CallistoIdConvention());
            ConventionPack.Add(new CallistoDateTimeConvention());
            ConventionPack.Add(new EnumRepresentationConvention(BsonType.String));
            ConventionPack.Add(new CallistoGuidConvention());
            ConventionPack.Add(new CallistoDecimalConvention());
            return this;
        }

        public ICallistoConventionRegistry ConfigureConventionPack(Action<ConventionPack> configurationAction)
        {
            if (configurationAction is null)
                throw new CallistoException("Cannot invoke a null action during the configuration of a convention pack.");
            configurationAction(ConventionPack);
            return this;
        }

        public void RegisterConventionPack()
        {
            RegisterConventionPack(_defaultName, _defaultFilter);
        }

        public void RegisterConventionPack(string name, Func<Type, bool> filter)
        {
            if(string.IsNullOrEmpty(name))
                throw new CallistoException("A convention pack must have a name.");
            if(filter == null)
                throw new CallistoException("A convention pack must have a filter. To apply the convention pack to all classes use '_ => true'. " +
                                            "Keep in mind that this is not advised.");
            ConventionRegistry.Register(name, ConventionPack, filter);
        }
    }
}
