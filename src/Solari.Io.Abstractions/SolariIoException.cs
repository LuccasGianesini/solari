using System;
using System.Runtime.Serialization;

namespace Solari.Io.Abstractions
{
    [Serializable]
    public class SolariIoException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public SolariIoException() { }
        public SolariIoException(string message) : base(message) { }
        public SolariIoException(string message, Exception inner) : base(message, inner) { }

        protected SolariIoException(SerializationInfo info,
                              StreamingContext context) : base(info, context)
        {
        }
    }
}