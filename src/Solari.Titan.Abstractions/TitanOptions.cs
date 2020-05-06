namespace Solari.Titan.Abstractions
{
    public class TitanOptions
    {
        public string DefaultLevel { get; set; }
        public FileOptions File { get; set; }
        public ConsoleOptions Console { get; set; }
        public OverridesOptions Overrides { get; set; } = new OverridesOptions();
        public SeqOptions Seq { get; set; }
        public GreyLogOptions GrayLog { get; set; }
    }
}