﻿using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Serializers;

namespace Solari.Callisto.Conventions
{
    public class CallistoDateTimeConvention : ConventionBase, IMemberMapConvention
    {
        public CallistoDateTimeConvention() : base("Solari.Callisto.DateTimeConvention")
        {
        }

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
            memberMap.SetSerializer(new DateTimeOffsetSerializerCustom(memberMap.MemberType, new DateTimeOffsetSerializer(BsonType.Document)));
            CallistoLogger.ConventionsLogger.DateTimeOffset(memberMap.ClassMap.ClassType.Name, memberMap.MemberName);
        }
    }
}
