using System.IO;
using Serilog;
using Serilog.Events;

namespace Solari.Titan.Abstractions
{
    public static class TitanLibHelper
    {
        public static string BuildPath(params string[] paths) { return Path.Combine(paths); }

        public static LogEventLevel GetLogLevel(string level)
        {
            if (string.IsNullOrEmpty(level)) return LogEventLevel.Information;
            return level.ToUpperInvariant() switch
                   {
                       "VERBOSE"     => LogEventLevel.Verbose,
                       "TRACE"       => LogEventLevel.Verbose,
                       "DEBUG"       => LogEventLevel.Debug,
                       "INFORMATION" => LogEventLevel.Information,
                       "INFO"        => LogEventLevel.Information,
                       "WARNING"     => LogEventLevel.Warning,
                       "WARN"        => LogEventLevel.Warning,
                       "ERROR"       => LogEventLevel.Error,
                       "ERR"         => LogEventLevel.Error,
                       "FATAL"       => LogEventLevel.Fatal,
                       _             => LogEventLevel.Information
                   };
        }

        public static RollingInterval GetRollingInterval(string fromSettings)
        {
            return fromSettings.ToUpperInvariant() switch
                   {
                       "INFINITE" => RollingInterval.Infinite,
                       "YEAR"     => RollingInterval.Year,
                       "MONTH"    => RollingInterval.Month,
                       "DAY"      => RollingInterval.Day,
                       "HOUR"     => RollingInterval.Hour,
                       "MINUTE"   => RollingInterval.Minute,
                       _          => RollingInterval.Day
                   };
        }
    }
}
