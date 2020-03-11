namespace Solari.Deimos.Abstractions
{
    public class DeimosOptions
    {
        public bool TracingEnabled { get; set; }
        public bool UseCorrelationId { get; set; }
        public bool UseJaeger { get; set; }
        public bool UseElasticApm { get; set; }
        public JaegerOptions Jaeger { get; set; } = new JaegerOptions();
        public HttpOptions Http { get; set; } = new HttpOptions();
        
        public ElasticApmOptions Elastic { get; set; }= new ElasticApmOptions();
        
    }
}