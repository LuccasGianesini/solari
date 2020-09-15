namespace Solari.Vanth
{
    public static class CommonErrorMessages
    {
        public const string ValidationError = "The provided object did not pass the validation.";
        public const string SerializationError = "Unable to serialize the object.";
        public const string DeserializationError = "Unable to deserialize the object.";
        public const string HttpError = "There was a problem executing the http request.";
        public static string NullResponseError(string operation) => $"The '{operation}' operation returned a null response.";
    }
}
