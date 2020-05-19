using System;
using System.Runtime.Serialization;

namespace Solari.Samples.Domain.Person.Exceptions
{
    [Serializable]
    public class AddPersonAttributeException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public AddPersonAttributeException() { }
        public AddPersonAttributeException(string message) : base(message) { }
        public AddPersonAttributeException(string message, Exception inner) : base(message, inner) { }

        protected AddPersonAttributeException(SerializationInfo info,
                                              StreamingContext context) : base(info, context)
        {
        }
    }
}