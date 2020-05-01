namespace Solari.Io.Abstractions
{
    public class IoSeqPublisher
    {
        public bool Enabled { get; set; }
        public bool UseTitanConfiguration { get; set; }
        public string IngestionEndpoint { get; set; }
        public string ApiKey { get; set; }
    }
}