using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Solari.Vanth
{
    [Serializable]
    public record Error : IError
    {
        public string Code { get; init; }
        public List<IErrorDetail> Details { get;}
        public string ErrorType { get; init;}
        public string Message { get; init;}
        public string Target { get; init;}


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Code), Code);
            info.AddValue(nameof(Details), Details);
            info.AddValue(nameof(ErrorType), ErrorType);
            info.AddValue(nameof(Message), Message);
            info.AddValue(nameof(Target), Target);

        }
    }
}
