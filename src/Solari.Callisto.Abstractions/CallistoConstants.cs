using MongoDB.Bson;

namespace Solari.Callisto.Abstractions
{
    public class CallistoConstants
    {
        public const string Health = "mongodb";
        private static readonly ObjectId Id = new ObjectId();
        public const string ConnectorAppSettingsSection = "Callisto";
        public const string TracerAppSettingsSection = "CallistoTracer";
        public const string ObjectIdDefaultValueAsString = "000000000000000000000000";
        public static ObjectId ObjectIdDefaultValue = Id;
        public const string TracerPrefix = "Callisto.MongoDb";
        public const string CallistoConnectorCacheKey = "callisto_connector";
    }
}