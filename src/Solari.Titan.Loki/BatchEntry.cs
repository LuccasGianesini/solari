using Newtonsoft.Json;

namespace Solari.Titan.Loki
{
    // Copied from Serilog.Sinks.Loki 
    // https://github.com/JosephWoodward/Serilog-Sinks-Loki
    // Thanks @JosephWoodward
    public class BatchEntry
    {
        public BatchEntry(string ts, string line)
        {
            Ts = ts;
            Line = line;
        }

        [JsonProperty("ts")]
        public string Ts { get; set; }

        [JsonProperty("line")]
        public string Line { get; set; }
    }
}