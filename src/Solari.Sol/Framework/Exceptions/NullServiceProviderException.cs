using System;
using System.Runtime.Serialization;

namespace Solari.Sol.Framework.Exceptions
{
    [Serializable]
    public class NullServiceProviderException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        private const string DefaultMessage = "The IServiceProvider has a null instance! Application cannot be started.";

        public NullServiceProviderException() : base(DefaultMessage) { }
        public NullServiceProviderException(string message) : base(message) { }
        public NullServiceProviderException(string message, Exception inner) : base(message, inner) { }

        protected NullServiceProviderException(SerializationInfo info,
                                               StreamingContext context) : base(info, context)
        {
        }
    }
}