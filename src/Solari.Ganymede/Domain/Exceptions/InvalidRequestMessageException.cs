using System;
using System.Runtime.Serialization;

namespace Solari.Ganymede.Domain.Exceptions
{
    [Serializable]
    public class InvalidRequestMessageException : Exception
    {
        public InvalidRequestMessageException() { }

        public InvalidRequestMessageException(string message) : base(message) { }

        public InvalidRequestMessageException(string message, Exception inner) : base(message, inner) { }

        protected InvalidRequestMessageException(SerializationInfo info,
                                                 StreamingContext context) : base(info, context)
        {
        }
    }
}