namespace Solari.Ganymede.Domain
{
    public static class HttpRequestMessageProperties
    {
        public const string CancellationTokenProperty = "GanymedeCancellationToken";
        public const string CompletionOptionProperty = "GanymedeCompletionOption";
        public const string ContentDeserializerProperty = "GanymedeContentDeserializer";
        public const string ContentSerializerProperty = "GanymedeContentSerializer";
        public const string RequestFormDataProperty = "GanymedeRquestFormData";
        public const string RequestMessageTimeout = "GanymedeRequestTimeout";
    }
}