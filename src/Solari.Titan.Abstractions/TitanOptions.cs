namespace Solari.Titan.Abstractions
{
    public class TitanOptions
    {
        public string DefaultLevel { get; set; }
        public LokiOptions Loki { get; set; }
        public FileOptions File { get; set; }
        public ConsoleOptions Console { get; set; }
        public OverridesOptions Overrides { get; set; } = new OverridesOptions();

    }
}