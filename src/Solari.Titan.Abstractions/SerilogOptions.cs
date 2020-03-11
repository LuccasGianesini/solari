using Serilog.Events;

namespace Solari.Titan.Abstractions
{
    public class SerilogOptions
    {
        /// <summary>
        /// Indicates if console logging is enabled.
        /// </summary>
        public bool UseConsole { get; set; }
        /// <summary>
        /// Indicates if log messages are supposed to be persisted in a file.
        /// </summary>
        public bool UseFile { get; set; }
        /// <summary>
        /// Indicates if the log messages should be sent to a Seq host.
        /// </summary>
        public bool UseSeq { get; set; }
        /// <summary>
        /// Indicates if the log messages should be sent to a GreyLog host. 
        /// </summary>
        public bool UseGreyLog { get; set; }
        
        public bool UseElk { get; set; }
        /// <summary>
        /// The default log level.
        /// </summary>
        public string DefaultLevel { get; set; }
        
        public string LogLevelRestriction { get; set; } = "Warning";
        public FileOptions File { get; set; } = new FileOptions();
        public OverridesOptions Overrides { get; set; } = new OverridesOptions();
        public SeqOptions Seq { get; set; } = new SeqOptions();
        
        public ElasticOptions Elk { get; set; } = new ElasticOptions();
        public GreyLogOptions GreyLog { get; set; } = new GreyLogOptions();
        
    }
}