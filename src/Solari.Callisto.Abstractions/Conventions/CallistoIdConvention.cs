using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Solari.Callisto.Abstractions.Conventions
{
    public class CallistoIdConvention : ConventionBase, IMemberMapConvention
    {
        public CallistoIdConvention() : base("Solari.Callisto.IdConvention") { }

        public void Apply(BsonMemberMap memberMap)
        {
            if (memberMap.MemberName != "Id" || memberMap.MemberType != typeof(string)) return;
            memberMap
                .SetIdGenerator(ObjectIdGenerator.Instance)
                .SetSerializer(new ObjectIdSerializer())
                .SetOrder(0)
                .SetElementName("_id");
            CallistoLogger.ConventionsLogger.Id(memberMap.ClassMap.ClassType.Name, memberMap.MemberName);
        }
    }
}