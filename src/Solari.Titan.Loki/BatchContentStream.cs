using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Serilog.Sinks.Loki.Labels;

namespace Solari.Titan.Loki
{
    // Copied from Serilog.Sinks.Loki 
    // https://github.com/JosephWoodward/Serilog-Sinks-Loki
    // Thanks @JosephWoodward
    public class BatchContentStream
    {
        [JsonIgnore]
        public List<LokiLabel> Labels { get; } = new List<LokiLabel>();

        [JsonProperty("labels")]
        public string LabelsString
        {
            get
            {
                StringBuilder sb = new StringBuilder("{");
                bool firstLabel = true;
                foreach (LokiLabel label in Labels)
                {
                    if (firstLabel)
                        firstLabel = false;
                    else
                        sb.Append(",");

                    sb.Append(label.Key);
                    sb.Append("=\"");
                    sb.Append(label.Value);
                    sb.Append("\"");
                }

                sb.Append("}");
                return sb.ToString();
            }
        }


        [JsonProperty("entries")]
        public List<BatchEntry> Entries { get; set; } = new List<BatchEntry>();
    }
}