using System;
using System.Runtime.Serialization;

namespace Solari.Callisto.Abstractions.Exceptions
{
    [Serializable]
    public class NullFilterDefinitionException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public NullFilterDefinitionException() { }
        public NullFilterDefinitionException(string message) : base(message) { }
        public NullFilterDefinitionException(string message, Exception inner) : base(message, inner) { }

        protected NullFilterDefinitionException(SerializationInfo info,
                                                StreamingContext context) : base(info, context)
        {
        }
    }
}