using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Solari.Titan.Loki
{
    // Copied from Serilog.Sinks.Loki 
    // https://github.com/JosephWoodward/Serilog-Sinks-Loki
    // Thanks @JosephWoodward
    public class BatchContent
    {
        [JsonProperty("streams")]
        public List<BatchContentStream> Streams { get; set; } = new List<BatchContentStream>();

        public string Serialize()
        {
            JsonSerializer serializer = new JsonSerializer();
            TextWriter writer = new StringWriter();
            serializer.Serialize(writer, this);
            return writer.ToString();
        }
    }
}