namespace Solari.Miranda.Abstractions.Options
{
    public class MirandaQueueOptions
    {
        public bool Declare { get; set; }
        public bool AutoDelete { get; set; }
        public bool Durable { get; set; } = true;
        public bool Exclusive { get; set; }
    }
}