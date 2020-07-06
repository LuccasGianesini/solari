using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using Solari.Callisto.Abstractions;

namespace Solari.Callisto.Conventions
{
    public class CallistoGuidConvention : ConventionBase, IMemberMapConvention
    {
        public CallistoGuidConvention() : base("Solari.Callisto.GuidConvention") { }

        public void Apply(BsonMemberMap memberMap)
        {
            if (memberMap.MemberType != typeof(Guid) && memberMap.MemberType != typeof(Guid?)) return;
            memberMap.SetSerializer(new GuidSerializer(BsonType.String));
            memberMap.ApplyDefaultValue(string.Empty);
            CallistoLogger.ConventionsLogger.Guid(memberMap.ClassMap.ClassType.Name, memberMap.MemberName);
        }
    }
}