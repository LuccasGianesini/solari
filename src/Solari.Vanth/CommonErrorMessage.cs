namespace Solari.Vanth
{
    public static class CommonErrorMessage
    {
        public const string ValidationError = "The provided object did not pass the validation.";
        public const string SerializationError = "Unable to serialize or deserializer the object.";
        public const string TransportError = "There was a problem with the transport used by the request.";
        public static string NullObjectError(string operation) => $"The '{operation}' operation received/returned a null object.";
        public static string IntegrationError(string service) => $"The integration with the service {service} failed.";
        public static string DatabaseError(string database) => $"The operation executed against the database '{database}' has failed.";
        public const string ExceptionError = "An exeception has been thrown.";
    }
}
