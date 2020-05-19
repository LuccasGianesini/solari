namespace Solari.Titan.Abstractions
{
    public class SeqOptions
    {
        public string LogLevelRestriction { get; set; } = "Warning";
        public bool Enabled { get; set; }
        public string Apikey { get; set; }
        public long EventBodySizeLimit { get; set; } = 5242880;
        public string IngestionEndpoint { get; set; }
        public string Period { get; set; } = "s30";
        public int QueueSizeLimit { get; set; } = 150;
        public long RawIngestionPayload { get; set; } = 20971520;
    }
}