using System.IO;
using Serilog;
using Serilog.Events;

namespace Solari.Titan.Framework
{
    public static class TitanLibHelper
    {
        public static string BuildPath(params string[] paths) { return Path.Combine(paths); }

        public static LogEventLevel GetLogLevel(string level)
        {
            if (string.IsNullOrEmpty(level)) return LogEventLevel.Information;
            return level.ToLowerInvariant() switch
                   {
                       "verbose"     => LogEventLevel.Verbose,
                       "trace"       => LogEventLevel.Verbose,
                       "debug"       => LogEventLevel.Debug,
                       "information" => LogEventLevel.Information,
                       "info"        => LogEventLevel.Information,
                       "warning"     => LogEventLevel.Warning,
                       "warn"        => LogEventLevel.Warning,
                       "error"       => LogEventLevel.Error,
                       "err"         => LogEventLevel.Error,
                       "fatal"       => LogEventLevel.Fatal,
                       "critical"    => LogEventLevel.Fatal,
                       _             => LogEventLevel.Information
                   };
        }

        public static RollingInterval GetRollingInterval(string fromSettings)
        {
            return fromSettings.ToLowerInvariant() switch
                   {
                       "infinite" => RollingInterval.Infinite,
                       "year"     => RollingInterval.Year,
                       "month"    => RollingInterval.Month,
                       "day"      => RollingInterval.Day,
                       "hour"     => RollingInterval.Hour,
                       "minute"   => RollingInterval.Minute,
                       _          => RollingInterval.Day
                   };
        }
    }
}