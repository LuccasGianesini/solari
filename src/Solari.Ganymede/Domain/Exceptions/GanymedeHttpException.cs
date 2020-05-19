using System;
using System.Net;

namespace Solari.Ganymede.Domain.Exceptions
{
    public class GanymedeException : Exception
    {
        public GanymedeException(string message) : base(message)
        {
            
        }
        public GanymedeException(string message, Exception innerException) : base(message, innerException) { }

        public GanymedeException(HttpStatusCode statusCode, string reasonPhrase, string message, Exception innerException)
            : base($"{message} Status code: {statusCode.ToString()} Reason phrase: {reasonPhrase} ", innerException)
        {
        }
    }
}