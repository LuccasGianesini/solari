using System;
using System.Runtime.Serialization;

namespace Solari.Callisto.Abstractions.Exceptions
{
    [Serializable]
    public class NullCallistoOperationException : Exception
    {
        public NullCallistoOperationException() { }
        public NullCallistoOperationException(string message) : base(message) { }
        public NullCallistoOperationException(string message, Exception inner) : base(message, inner) { }

        protected NullCallistoOperationException(SerializationInfo info,
                                                 StreamingContext context) : base(info, context)
        {
        }
    }
}