using System;
using System.Runtime.Serialization;

namespace Solari.Ganymede.Domain.Exceptions
{
    [Serializable]
    public class GanymedeMessageValidationException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public GanymedeMessageValidationException() { }

        public GanymedeMessageValidationException(string message) : base(message) { }

        public GanymedeMessageValidationException(string message, Exception inner) : base(message, inner) { }

        protected GanymedeMessageValidationException(SerializationInfo info,
                                             StreamingContext context) : base(info, context)
        {
        }
    }
}