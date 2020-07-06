using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using Solari.Callisto.Abstractions;

namespace Solari.Callisto.Conventions
{
    public class CallistoIgnoreExtraElementsConvention : ConventionBase, IClassMapConvention
    {
        public CallistoIgnoreExtraElementsConvention() : base("Solari.Callisto.IgnoreExtraElementsConvention") { }

        public void Apply(BsonClassMap classMap)
        {
            classMap.SetIgnoreExtraElements(true);
            classMap.SetIgnoreExtraElementsIsInherited(true);
            CallistoLogger.ConventionsLogger.Ignore(classMap.ClassType.Name);
        }
    }
}