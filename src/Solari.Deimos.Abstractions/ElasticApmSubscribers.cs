namespace Solari.Deimos.Abstractions
{
    public class ElasticApmSubscribers
    {
        public bool Http { get; set; }
        public bool AspNetCore { get; set; }
        public bool EfCore { get; set; }
    }
}