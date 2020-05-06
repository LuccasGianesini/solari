using System;
using System.Runtime.Serialization;

namespace Solari.Hyperion.ConfigurationProvider
{
    [Serializable]
    public class HyperionLoadException : Exception
    {
        public HyperionLoadException() { }
        public HyperionLoadException(string message) : base(message) { }
        public HyperionLoadException(string message, Exception inner) : base(message, inner) { }

        protected HyperionLoadException(SerializationInfo info,
                                        StreamingContext context) : base(info, context)
        {
        }
    }
}