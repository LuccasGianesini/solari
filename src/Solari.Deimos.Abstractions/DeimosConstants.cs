namespace Solari.Deimos.Abstractions
{
    public static class DeimosConstants
    {
        public const string TracingAppSettingsSection = "Deimos";
        public const string MessageIdHeader = "x-message-id";
        public const string RequestIdHeader = "x-request-id";
        public const string EnvoyTraceIdHeader = "x-b3-traceid";
        public const string EnvoySpanIdHeader = "x-b3-spanid";
        public const string EnvoyParentSpanIdHeader = "x-b3-parentspanid";
        public const string EnvoySampledHeader = "x-b3-sampled";
        public const string EnvoyFlagsHeader = "x-b3-flags";
        public const string EnvoyOutgoingSpanContext = "x-ot-span-context";

        public static string[] Headers { get; } =
        {
            MessageIdHeader,
            EnvoyFlagsHeader,
            EnvoySampledHeader,
            RequestIdHeader,
            EnvoyOutgoingSpanContext,
            EnvoySpanIdHeader,
            EnvoyTraceIdHeader,
            EnvoyParentSpanIdHeader
        };
    }
}