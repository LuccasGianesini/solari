using System;
using System.Runtime.Serialization;

namespace Solari.Deimos.CorrelationId
{
    [Serializable]
    public class DioneException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public DioneException() { }
        public DioneException(string message) : base(message) { }
        public DioneException(string message, Exception inner) : base(message, inner) { }

        protected DioneException(SerializationInfo info,
                              StreamingContext context) : base(info, context)
        {
        }
    }
}