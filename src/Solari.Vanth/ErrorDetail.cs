using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Solari.Vanth
{
    [Serializable]
    public record ErrorDetail : IErrorDetail
    {
        public string Code { get; init; }
        public string StackTrace { get; init; }
        public string HelpUrl { get; init; }
        public string Message { get; init; }
        public string Target { get; init; }
        public string Source { get; init; }

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
