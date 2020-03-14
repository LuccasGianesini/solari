using System;
using System.Runtime.Serialization;

namespace Solari.Vanth.Exceptions
{
    [Serializable]
    public class NullOrEmptyErrorMessageException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public NullOrEmptyErrorMessageException() : base("An error must contain an error message") { }
        public NullOrEmptyErrorMessageException(string message) : base(message) { }
        public NullOrEmptyErrorMessageException(string message, Exception inner) : base(message, inner) { }

        protected NullOrEmptyErrorMessageException(SerializationInfo info,
                              StreamingContext context) : base(info, context)
        {
        }
    }
}