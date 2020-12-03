using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Solari.Vanth
{
    [Serializable]
    public class Error : IError, ISerializable
    {
        public void TESTE()
        {
            var res = new Result<int>();
        }
        public string Code { get; set; }
        public List<IErrorDetail> Details { get; set;} = new List<IErrorDetail>(2);
        public string ErrorType { get; set;}
        public string Message { get; set;}
        public string Target { get; set;}


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.SetType(this.GetType());
            info.FullTypeName = this.GetType().FullName;
            info.AddValue(nameof(Code), Code);
            info.AddValue(nameof(Details), Details);
            info.AddValue(nameof(ErrorType), ErrorType);
            info.AddValue(nameof(Message), Message);
            info.AddValue(nameof(Target), Target);

        }
    }
}
