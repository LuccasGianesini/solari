using System;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using Solari.Callisto.Abstractions.Serializers;

namespace Solari.Callisto.Abstractions.Conventions
{
    public class CallistoDateTimeConvention : ConventionBase, IMemberMapConvention
    {
        public CallistoDateTimeConvention() : base("Solari.Callisto.DateTimeConvention") { }

        public void Apply(BsonMemberMap memberMap)
        {
            MapDateTime(memberMap);
            MapDateTimeOffset(memberMap);
        }

        private static void MapDateTime(BsonMemberMap memberMap)
        {
            if (memberMap.MemberType != typeof(DateTime) && memberMap.MemberType != typeof(DateTime?)) return;
            memberMap.SetSerializer(new DateTimeSerializerCustom());
            CallistoLogger.ConventionsLogger.DateTime(memberMap.ClassMap.ClassType.Name, memberMap.MemberName);
        }

        private static void MapDateTimeOffset(BsonMemberMap memberMap)
        {
            if (memberMap.MemberType != typeof(DateTimeOffset) && memberMap.MemberType != typeof(DateTimeOffset?)) return;
            memberMap.SetSerializer(new DateTimeOffsetSerializerCustom());
            CallistoLogger.ConventionsLogger.DateTimeOffset(memberMap.ClassMap.ClassType.Name, memberMap.MemberName);
        }
    }
}