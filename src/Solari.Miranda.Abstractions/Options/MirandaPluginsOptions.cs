namespace Solari.Miranda.Abstractions.Options
{
    public class MirandaPluginsOptions
    {
        public bool UsePolly { get; set; } = true;
        public bool UseProtoBuf { get; set; } = true;
        public bool UseTracing { get; set; } = true;
    }
}