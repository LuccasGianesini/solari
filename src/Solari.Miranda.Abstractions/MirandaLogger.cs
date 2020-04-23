using Serilog;

namespace Solari.Miranda.Abstractions
{
    public static class MirandaLogger
    {
        public static class JaegerTracingPlugin
        {
            private const string Prefix = "Solari.Miranda.Tracer (Tracer): ";

            public static void LogSkippingMessage(string messageId) => Log.Debug($"{Prefix}Skipping message tracing. MessageId: {messageId}");
            public static void LogMessageContext(string messageContext) => Log.Debug($"{Prefix}A scope with span-context: {messageContext}, was created");

            public static void LogPrecessingMessage(string messageName, string messageId) =>
                Log.Debug($"`{Prefix}Started processing: {messageName} [id: {messageId}]");

            public static void LogFinishedProcessingMessage(string messageName, string messageId) =>
                Log.Debug($"{Prefix}Finished processing: {messageName} [id: {messageId}]");

            public static void LogExceptionWhileCreatingScope(string message) => Log.Fatal($"{Prefix}An exception while creating tracing scope: {message}");
        }

        public static class RedisMessageProcessor
        {
            private const string Prefix = "Solari.Miranda (RedisProcessor):";
            public static void LogMessageNotInCache(string key) => Log.Debug($"{Prefix}Message is not present in redis. Skipping [key: {key}]");

            public static void LogMessageWithNoExpiry(string key) => Log.Warning($"{Prefix}Setting message into redis cache with no expiration. [key: {key}]");

            public static void LogMessageWithExpiry(string key, int expiry) =>
                Log.Debug($"{Prefix}Setting message into redis cache with expiration of {expiry}. [key: {key}]");

            public static void LogReceivedUniqueMessage(string id) => Log.Verbose($"Received a unique message with id to be processed. [id: {id}]");

            public static void LogMessageWasAlreadyProcessed(string id) => Log.Verbose($"A unique message was already processed. [id:{id}]");

            public static void LogProcessingUniqueMessage(string id) => Log.Debug($"Processing a unique message. [id: {id}]");
            public static void LogPrecessedUniqueMessage(string id) => Log.Debug($"Processed a unique message. [id: {id}]");

            public static void LogErrorProcessingUniqueMessage(string id) => Log.Debug($"There was an error when processing a unique message. [id: {id}]");
        }

        public static class DependencyInjection
        {
            private const string Prefix = "Solari.Miranda (InstanceFactory):";

            public static void LogRegisteringJaegerMiddleware() => Log.Debug($"{Prefix}Registering jaeger tracing staged middleware");
            public static void LogRegisteringMessageProcessorMiddleware() => Log.Debug($"{Prefix}Registering message processor middleware");
            public static void LogRegisteringProtobufMiddleware() => Log.Debug($"{Prefix}Registering protobuf middleware");

            public static void LogMessageProcessorSelector(string messageProcessor)
            {
                if (string.IsNullOrEmpty(messageProcessor))
                {
                    Log.Debug($"{Prefix}Using empty message processor");
                }
                else
                {
                    Log.Debug($"{Prefix}Using {messageProcessor} message processor");
                }
            }
        }
    }
}