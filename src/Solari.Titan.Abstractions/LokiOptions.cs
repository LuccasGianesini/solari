namespace Solari.Titan.Abstractions
{
    public class LokiOptions
    {
        public bool Enabled { get; set; }
        public string RpcEndpoint { get; set; }
        public string LogLevelRestriction { get; set; } = "Warning";
        public int QueueLimit { get; set; } = 5;
        public int BatchSizeLimit { get; set; } = 1000;
        public string Period { get; set; } = "s10";
        public bool StackTraceAsLabel { get; set; }

    }
    
    
}