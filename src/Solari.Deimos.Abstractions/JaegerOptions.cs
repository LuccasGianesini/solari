namespace Solari.Deimos.Abstractions
{
    public class JaegerOptions
    {
        public bool Enabled { get; set; }
        public int MaxPacketSize { get; set; } = 0;
        public double MaxTracesPerSecond { get; set; } = 7;
        public string Sampler { get; set; }
        public double SamplingRate { get; set; } = 0.2;
        public string ServiceName { get; set; }
        public string UdpHost { get; set; }
        public int UdpPort { get; set; } = 6831;
    }
}