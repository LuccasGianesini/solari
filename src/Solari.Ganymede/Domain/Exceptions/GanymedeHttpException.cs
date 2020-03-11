using System;
using System.Net;

namespace Solari.Ganymede.Domain.Exceptions
{
    public class SolariHttpException : Exception
    {
        public SolariHttpException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public SolariHttpException(HttpStatusCode statusCode, string reasonPhrase, string message, Exception innerException)
            : base($"{message} Status code: {statusCode.ToString()} Reason phrase: {reasonPhrase} ", innerException)
        {
        }
    }
}