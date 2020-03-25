namespace Solari.Miranda.Abstractions.Options
{
    public class MirandaQueueOptions
    {
        public bool AutoDelete { get; set; }
        public bool Durable { get; set; } = true;
        public bool Exclusive { get; set; }
    }
}