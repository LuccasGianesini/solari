using System;
using System.Runtime.Serialization;

namespace Solari.Juno.Abstractions
{
    [Serializable]
    public class JunoException : Exception
    {
        public string Key { get; }
        public JunoException() { }
        public JunoException(string message) : base(message) { }

        public JunoException(string message, Exception inner, string key) : base(message, inner) { Key = key; }

        protected JunoException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}