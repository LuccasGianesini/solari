using System;
using System.Runtime.Serialization;

namespace Solari.Callisto.Abstractions.Exceptions
{
    [Serializable]
    public class CallistoException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public CallistoException() { }
        public CallistoException(string message) : base(message) { }
        public CallistoException(string message, Exception inner) : base(message, inner) { }

        protected CallistoException(SerializationInfo info,
                              StreamingContext context) : base(info, context)
        {
        }
    }
}