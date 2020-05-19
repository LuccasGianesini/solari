using MongoDB.Bson;

namespace Solari.Callisto.Abstractions
{
    public class CallistoConstants
    {
        public const string Health = "mongodb";
        public const string ConnectorAppSettingsSection = "Callisto";
        public const string TracerAppSettingsSection = "CallistoTracer";
        public const string ObjectIdDefaultValueAsString = "000000000000000000000000";
        public const string TracerPrefix = "Callisto.MongoDb";
        public const string CallistoConnectorCacheKey = "callisto_connector";
        private static readonly ObjectId Id = new ObjectId();
        public static ObjectId ObjectIdDefaultValue = Id;
    }
}