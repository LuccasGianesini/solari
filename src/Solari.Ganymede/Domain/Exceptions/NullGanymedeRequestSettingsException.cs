using System;
using System.Runtime.Serialization;

namespace Solari.Ganymede.Domain.Exceptions
{
    [Serializable]
    public class NullGanymedeRequestSettingsException : Exception
    {
        public NullGanymedeRequestSettingsException() { }

        public NullGanymedeRequestSettingsException(string message) : base(message) { }

        public NullGanymedeRequestSettingsException(string message, Exception inner) : base(message, inner) { }

        protected NullGanymedeRequestSettingsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}