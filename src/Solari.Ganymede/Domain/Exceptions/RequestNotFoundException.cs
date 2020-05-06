using System;
using System.Runtime.Serialization;

namespace Solari.Ganymede.Domain.Exceptions
{
    [Serializable]
    public class RequestNotFoundException : Exception
    {
        public RequestNotFoundException() { }

        public RequestNotFoundException(string message) : base(message) { }

        public RequestNotFoundException(string message, Exception inner) : base(message, inner) { }

        protected RequestNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}