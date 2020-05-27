using System;
using System.Runtime.Serialization;

namespace Solari.Vanth
{
    [Serializable]
    public class VanthException : Exception
    {
        public VanthException() { }
        public VanthException(string message) : base(message) { }
        public VanthException(string message, Exception inner) : base(message, inner) { }

        protected VanthException(SerializationInfo info,
                              StreamingContext context) : base(info, context)
        {
        }
    }
}