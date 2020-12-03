using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Solari.Vanth
{
    [Serializable]
    public class ErrorDetail : IErrorDetail, ISerializable
    {
        public string Code { get; set; }
        public string StackTrace { get; set; }
        public string HelpUrl { get; set; }
        public string Message { get; set; }
        public string Target { get; set; }
        public string Source { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.SetType(this.GetType());
            info.FullTypeName = this.GetType().FullName;
            info.AddValue(nameof(Code), Code);
            info.AddValue(nameof(StackTrace), StackTrace);
            info.AddValue(nameof(HelpUrl), HelpUrl);
            info.AddValue(nameof(Message), Message);
            info.AddValue(nameof(Target), Target);
            info.AddValue(nameof(Source), Source);
        }
    }
}
