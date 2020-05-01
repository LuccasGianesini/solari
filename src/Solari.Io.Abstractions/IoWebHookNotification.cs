namespace Solari.Io.Abstractions
{
    public class IoWebHookNotification
    {
        public string Name { get; set; }

        public string Uri { get; set; }

        public string Payload { get; set; }

        public string RestoredPayload { get; set; }
    }
}