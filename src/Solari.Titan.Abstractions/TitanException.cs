﻿using System;
using System.Runtime.Serialization;

namespace Solari.Titan.Abstractions
{
    [Serializable]
    public class TitanException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public TitanException() { }
        public TitanException(string message) : base(message) { }
        public TitanException(string message, Exception inner) : base(message, inner) { }

        protected TitanException(SerializationInfo info,
                              StreamingContext context) : base(info, context)
        {
        }
    }
}