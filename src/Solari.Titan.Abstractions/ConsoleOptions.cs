using Serilog.Sinks.SystemConsole.Themes;

namespace Solari.Titan.Abstractions
{
    public class ConsoleOptions
    {
        public bool Enabled { get; set; }
        public string OutputTemplate { get; set; } = "[{Timestamp:yyyy-MM-dd HH:mm:ss}][{Level:u3}]{Message:lj} {NewLine}{Exception}";
        public string Theme { get; set; }

        public ConsoleTheme GetConsoleTheme()
        {
            if (string.IsNullOrEmpty(Theme)) return SystemConsoleTheme.Colored;

            return Theme.ToLowerInvariant() switch
                   {
                       "literate" => SystemConsoleTheme.Literate,
                       "gray"     => SystemConsoleTheme.Grayscale,
                       "colored"  => SystemConsoleTheme.Colored,
                       _          => SystemConsoleTheme.Colored
                   };
        }
    }
}