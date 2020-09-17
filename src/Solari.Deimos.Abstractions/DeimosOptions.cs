namespace Solari.Deimos.Abstractions
{
    public class DeimosOptions
    {
        public bool Enabled { get; set; }
        public JaegerOptions Jaeger { get; set; } = new JaegerOptions();
        public HttpOptions Http { get; set; } = new HttpOptions();
    }
}
