using Jaeger.Samplers;

namespace Solari.Deimos.Abstractions
{
    public class JaegerOptions
    {
        public int MaxPacketSize { get; set; } = 65000;
        public double MaxTracesPerSecond { get; set; } = 7;
        public string Sampler { get; set; }
        public double SamplingRate { get; set; } = 0.1;
        public string UdpHost { get; set; }
        public int UdpPort { get; set; } = 6831;

        public ISampler GetSampler()
        {
            if (string.IsNullOrEmpty(Sampler))
                return new ConstSampler(true);
            return Sampler switch
                   {
                       "const"         => new ConstSampler(true),
                       "rate"          => new RateLimitingSampler(MaxTracesPerSecond),
                       "probabilistic" => new ProbabilisticSampler(SamplingRate),
                       _               => new ConstSampler(true)
                   };
        }
    }
}