using System;
using System.Runtime.Serialization;

namespace Solari.Ceres.Abstractions
{
    [Serializable]
    public class CeresException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public CeresException() { }
        public CeresException(string message) : base(message) { }
        public CeresException(string message, Exception inner) : base(message, inner) { }

        protected CeresException(SerializationInfo info,
                              StreamingContext context) : base(info, context)
        {
        }
    }
}