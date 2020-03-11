using System;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using Solari.Callisto.Abstractions.Conventions;

namespace Solari.Callisto.Framework
{
    internal class CallistoConventionRegistry
    {
        private static readonly CallistoConventionRegistry _instance = new CallistoConventionRegistry();
        
        static CallistoConventionRegistry(){}

        private CallistoConventionRegistry()
        {
            RegisteredConventions = new Queue<IConvention>(10);
        }

        public static CallistoConventionRegistry Instance => _instance;

        internal Queue<IConvention> RegisteredConventions { get; }
        
        public CallistoConventionRegistry AddConvention(IConvention convention)
        {
            if (convention == null) throw new ArgumentNullException(nameof(convention));
            RegisteredConventions.Enqueue(convention);
            return this;
        }

        public void AddDefaultConventions()
        {
            RegisteredConventions.Enqueue(new ReadWriteMemberFinderConvention());
            RegisteredConventions.Enqueue(new CallistoIgnoreExtraElementsConvention());
            RegisteredConventions.Enqueue(new CallistoIdConvention());
            RegisteredConventions.Enqueue(new CallistoDateTimeConvention());
            RegisteredConventions.Enqueue(new EnumRepresentationConvention(BsonType.String));
            RegisteredConventions.Enqueue(new CallistoGuidConvention());
            RegisteredConventions.Enqueue(new CallistoDecimalConvention());
            
        }
        
        
    }
}