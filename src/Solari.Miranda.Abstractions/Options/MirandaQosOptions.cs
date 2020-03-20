namespace Solari.Miranda.Abstractions.Options
{
    public class MirandaQosOptions
    {
        public uint PrefetchSize { get; set; }
        public ushort PrefetchCount { get; set; }
        public bool Global { get; set; }
    }
}