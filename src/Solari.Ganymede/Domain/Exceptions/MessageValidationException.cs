using System;
using System.Runtime.Serialization;

namespace Solari.Ganymede.Domain.Exceptions
{
    [Serializable]
    public class MessageValidationException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public MessageValidationException() { }

        public MessageValidationException(string message) : base(message) { }

        public MessageValidationException(string message, Exception inner) : base(message, inner) { }

        protected MessageValidationException(SerializationInfo info,
                                             StreamingContext context) : base(info, context)
        {
        }
    }
}