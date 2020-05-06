using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace Solari.Callisto.Abstractions.Conventions
{
    public class CallistoDecimalConvention : ConventionBase, IMemberMapConvention
    {
        public CallistoDecimalConvention() : base("Solari.Callisto.DecimalConvention") { }

        public void Apply(BsonMemberMap memberMap)
        {
            if (memberMap.MemberType == typeof(decimal))
            {
                memberMap.SetSerializer(new DecimalSerializer(BsonType.Decimal128));
                CallistoLogger.ConventionsLogger.Decimal(memberMap.ClassMap.ClassType.Name, memberMap.MemberName);
            }

            if (memberMap.MemberType != typeof(decimal?)) return;
            memberMap.SetSerializer(new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));
            CallistoLogger.ConventionsLogger.Decimal(memberMap.ClassMap.ClassType.Name, memberMap.MemberName);
        }
    }
}