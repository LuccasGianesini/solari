using System;
using System.Runtime.Serialization;

namespace Solari.Oberon
{
    [Serializable]
    public class CacheSaveException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public CacheSaveException() { }
        public CacheSaveException(string message) : base(message) { }
        public CacheSaveException(string message, Exception inner) : base(message, inner) { }

        protected CacheSaveException(SerializationInfo info,
                                     StreamingContext context) : base(info, context)
        {
        }
    }
}