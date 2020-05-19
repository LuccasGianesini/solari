using System;
using System.Runtime.Serialization;

namespace Solari.Titan.Abstractions
{
    [Serializable]
    public class TitanException : Exception
    {

        public TitanException() { }
        public TitanException(string message) : base(message) { }
        public TitanException(string message, Exception inner) : base(message, inner) { }

        protected TitanException(SerializationInfo info,
                              StreamingContext context) : base(info, context)
        {
        }
    }
}