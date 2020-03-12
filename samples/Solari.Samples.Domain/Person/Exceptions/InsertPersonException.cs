using System;
using System.Runtime.Serialization;

namespace Solari.Samples.Domain.Person.Exceptions
{
    [Serializable]
    public class InsertPersonException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public InsertPersonException() { }
        public InsertPersonException(string message) : base(message) { }
        public InsertPersonException(string message, Exception inner) : base(message, inner) { }

        protected InsertPersonException(SerializationInfo info,
                              StreamingContext context) : base(info, context)
        {
        }
    }
}