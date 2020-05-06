using System;
using System.Runtime.Serialization;

namespace Solari.Ganymede.Domain.Exceptions
{
    [Serializable]
    public class RequestResourceNotFoundException : Exception
    {
        public RequestResourceNotFoundException() { }

        public RequestResourceNotFoundException(string message)
            : base(message)
        {
        }

        public RequestResourceNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected RequestResourceNotFoundException(SerializationInfo info,
                                                   StreamingContext context)
            : base(info, context)
        {
        }
    }
}