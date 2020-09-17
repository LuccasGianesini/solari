namespace Solari.Titan.Abstractions
{
    public class LokiOptions
    {
        public bool Enabled { get; set; }
        public string Endpoint { get; set; }
        public LokiCredentialsOptions Credentials { get; set; }
        public string LogLevelRestriction { get; set; } = TitanConstants.GlobalLogLevelRestriction;
        public int QueueLimit { get; set; } = 5;
        public int BatchSizeLimit { get; set; } = 1000;
        public string Period { get; set; } = "s10";


    }
}
