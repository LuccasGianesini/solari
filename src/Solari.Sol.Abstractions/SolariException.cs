using System;
using System.Runtime.Serialization;

namespace Solari.Sol.Abstractions
{
    [Serializable]
    public class SolariException : Exception
    {
        public SolariException()
        {
        }

        public SolariException(string message) : base(message)
        {
        }

        public SolariException(string message, Exception inner) : base(message, inner)
        {
        }

        protected SolariException(SerializationInfo info,
                               StreamingContext context) : base(info, context)
        {
        }
    }
}
