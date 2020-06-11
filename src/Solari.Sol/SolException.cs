using System;
using System.Runtime.Serialization;

namespace Solari.Sol
{
    [Serializable]
    public class SolException : Exception
    {
        public SolException()
        {
        }

        public SolException(string message) : base(message)
        {
        }

        public SolException(string message, Exception inner) : base(message, inner)
        {
        }

        protected SolException(SerializationInfo info,
                               StreamingContext context) : base(info, context)
        {
        }
    }
}
