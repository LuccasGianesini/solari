using System;
using System.Runtime.Serialization;

namespace Solari.Deimos.Abstractions
{
    [Serializable]
    public class DeimosException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public DeimosException() { }
        public DeimosException(string message) : base(message) { }
        public DeimosException(string message, Exception inner) : base(message, inner) { }

        protected DeimosException(SerializationInfo info,
                              StreamingContext context) : base(info, context)
        {
        }
    }
}