namespace Solari.Miranda.Abstractions.Options
{
    public class MirandaMessageProcessorOptions
    {
        public bool Enabled { get; set; }
        public string Type { get; set; }
        public int MessageExpirySeconds { get; set; }
    }
}