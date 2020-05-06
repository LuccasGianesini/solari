namespace Solari.Oberon
{
    public class OberonOptions
    {
        public bool Enabled { get; set; }
        public string ConnectionString { get; set; }
        public string Instance { get; set; }

        public bool SerializeObjectsToJson { get; set; } = true;
    }
}