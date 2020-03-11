using Serilog;

namespace Solari.Deimos.Abstractions
{
    public static class DeimosLogger
    {
        public static class CorrelationIdLogger
        {
            private const string Prefix = "Solari.Deimos (CorrelationId): ";

            public static void UsingCorrelationIdMiddleware() => Log.Debug($"{Prefix}Application is using the CorrelationId middleware");

            public static void CreatedHttpCorrelationContext(string correlationId) =>
                Log.Debug($"{Prefix}Creating http correlation context with CorrelationId: {correlationId}");

            public static void CreatedBrokerCorrelationContext(string correlationId) =>
                Log.Debug($"{Prefix}Creating broker correlation context with CorrelationId: {correlationId}");

            public static void TryGetCorrelationId() => Log.Debug($"{Prefix}Trying to get http request CorrelationId value");
            public static void GotCorrelationIdValue(string correlationId) => Log.Debug($"{Prefix}Got CorrelationId: {correlationId}");
            public static void AddCorrelationIdInResponse() => Log.Debug($"{Prefix}Added CorrelationId into response headers");

            public static void InvokedMiddleware() => Log.Debug($"{Prefix}Invoked CorrelationId middleware");
        }

        public class ElasticLogger
        {
            private const string Prefix = "Solari.Deimos (ElasticApm): ";
            public static void UsingAspNetCoreSubscriber() => Log.Debug($"{Prefix}Application is using AspNetCore subscriber");
            public static void UsingHttpSubscriber() => Log.Debug($"{Prefix}Application is using outgoing Http subscriber");
            public static void UsingEfCoreSubscriber() => Log.Debug($"{Prefix}Application is using EfCore subscriber");
            public static void UsingElasticApmTracer() => Log.Debug($"{Prefix}Application is using ElasticApm tracing infrastructure");
            public static void ApmServerAddress(string address) => Log.Debug($"{Prefix}ElasticApm server address: {address}");
            public static void ConfiguredTracer() => Log.Debug($"{Prefix}Configured ElasticApm tracer using environment variables");
        }

        public static class JaegerLogger
        {
            private const string Prefix = "Solari.Deimos (Jaeger): ";
            public static void ConfiguredHttpOut() => Log.Debug("Configured Jaeger outgoing http tracing");
            public static void ConfiguredHttpIn() => Log.Debug("Configured Jaeger incoming http tracing");
            public static void UdpRemoteReporter(string host, int port) => Log.Debug($"{Prefix}Udp remote reporter is listening on {host}:{port}");
            public static void ConfiguredTracer() => Log.Debug($"{Prefix}Configured Jaeger tracer using app-settings");

            public static void ConfigureRequestFiltering(string endpoint) =>
                Log.Debug($"{Prefix}Requests to the following endpoint will not be traced: {endpoint}");
            public static void UsingJaegerTracing() => Log.Debug($"{Prefix}Application is using Jaeger tracing infrastructure");
        }
    }
}