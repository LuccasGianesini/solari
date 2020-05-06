using System;
using System.Runtime.Serialization;

namespace Solari.Callisto.Abstractions.Exceptions
{
    [Serializable]
    public class NullPipelineDefinitionException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public NullPipelineDefinitionException() { }
        public NullPipelineDefinitionException(string message) : base(message) { }
        public NullPipelineDefinitionException(string message, Exception inner) : base(message, inner) { }

        protected NullPipelineDefinitionException(SerializationInfo info,
                                                  StreamingContext context) : base(info, context)
        {
        }
    }
}