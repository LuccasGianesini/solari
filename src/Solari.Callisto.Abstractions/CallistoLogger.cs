using Serilog;

namespace Solari.Callisto.Abstractions
{
    public static class CallistoLogger
    {
        public static class ConnectionLogger
        {
            public const string Prefix = "Solari.Callisto (Connection): ";

            public static void UpdatingMongoClient()
            {
                Log.Debug($"{Prefix}Updating the current MongoClient. A connection with different settings was created. The current stored client will be updated");
            }

            public static void AddingMongoClient()
            {
                Log.Debug($"{Prefix}Adding a IMongoClient into the dictionary");
            }

            public static void ChangingDatabase(string database)
            {
                Log.Debug($"{Prefix}Changing the current Database to {database}");
            }

            public static void ConnectionStatus(string connectionState, string pingResult)
            {
                Log.Debug($"{Prefix}MongoDb connection state is: {connectionState}. Ping result is: {pingResult}");
            }

            public static void CreatingConnection()
            {
                Log.Debug($"{Prefix}Creating callisto mongodb connection");
            }
        }

        public static class ConventionPackLogger
        {
            private const string Prefix = "Solari.Callisto (ConventionPack): ";

            public static void UsingDefaultConventions()
            {
                Log.Debug($"{Prefix}Using default conventions");
            }

            public static void RegisteringConvention(string conventionName)
            {
                Log.Debug($"{Prefix}Registering convention {conventionName}");
            }

            public static void RegisterConventionPack(string conventionPackName)
            {
                Log.Debug($"{Prefix}Registered convention pack with name {conventionPackName}");
            }
        }

        public static class ClassMapsLogger
        {
            private const string Prefix = "Solari.Callisto (ClassMap): ";

            public static void UsingDefaultClassMaps()
            {
                Log.Debug($"{Prefix}Using default ClassMaps");
            }

            public static void IdentifiedType(string className)
            {
                Log.Debug($"{Prefix}Got callisto type with name '{className}'");
            }
            public static void RegisteredClassMap(string fullName)
            {
                Log.Debug($"{Prefix}Registered a class map for type {fullName}");
            }
        }

        public static class CollectionLogger
        {
            private const string Prefix = "Solari.Callisto (CollectionContext): ";

            public static void Registering(string collectionName, string lifeTime)
            {
                Log.Debug($"{Prefix}Registering {collectionName} repository with {lifeTime} lifetime");
            }

            public static void RegisteringCollection(string collectionName, string lifeTime)
            {
                Log.Debug($"{Prefix}Registering collection {collectionName} with {lifeTime} lifetime");
            }
        }

        public static class ConventionsLogger
        {
            private const string Prefix = "Solari.Callisto (Convention): ";

            public static void DateTime(string className, string memberName)
            {
                Log.Debug($"{Prefix}Applying DateTime convention to member {memberName} of {className}");
            }

            public static void DateTimeOffset(string className, string memberName)
            {
                Log.Debug($"{Prefix}Applying DateTimeOffset convention to member {memberName} of {className}");
            }

            public static void Decimal(string className, string memberName)
            {
                Log.Debug($"{Prefix}Applying Decimal convention to member {memberName} of {className}");
            }

            public static void Guid(string className, string memberName)
            {
                Log.Debug($"{Prefix}Applying Guid convention to member {memberName} of {className}");
            }

            public static void Id(string className, string memberName)
            {
                Log.Debug($"{Prefix}Applying Id convention to member {memberName} of {className}");
            }

            public static void Ignore(string className)
            {
                Log.Debug($"{Prefix}Applying IgnoreExtraElements convention to document {className}");
            }
        }
    }
}
