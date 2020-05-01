namespace Solari.Io.Abstractions
{
    public class IoPrometheusPublisher
    {
        public bool Enabled { get; set; }
        public string Address { get; set; }
        public string Instance { get; set; }
    }
}