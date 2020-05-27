using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.Http;
using Serilog.Sinks.Loki.Labels;
using System.Text;

namespace Solari.Titan.Loki
{
    // Copied from Serilog.Sinks.Loki 
    // https://github.com/JosephWoodward/Serilog-Sinks-Loki
    // Thanks @JosephWoodward
    internal class BatchFormatter : IBatchFormatter
    {
        // private readonly int _defaultLogLevelRestriction;
        private readonly IList<LokiLabel> _globalLabels;

        public BatchFormatter()
        {
            // _defaultLogLevelRestriction = GetLevelRestriction(logEventLevel);
            _globalLabels = new List<LokiLabel>();
        }

        public BatchFormatter(IList<LokiLabel> globalLabels)
        {
            // _defaultLogLevelRestriction = GetLevelRestriction(logEventLevel);
            _globalLabels = globalLabels;
        }

        public void Format(IEnumerable<LogEvent> logEvents, ITextFormatter formatter, TextWriter output)
        {
            if (logEvents == null)
                throw new ArgumentNullException(nameof(logEvents));
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            List<LogEvent> logs = logEvents.ToList();
            if (!logs.Any())
                return;

            var content = new BatchContent();
            foreach (LogEvent logEvent in logs)
            {
                // if (GetLevelRestriction(logEvent.Level) < _defaultLogLevelRestriction)
                //     continue;
                var stream = new BatchContentStream();
                content.Streams.Add(stream);

                stream.Labels.Add(new LokiLabel("level", GetLevel(logEvent.Level)));
                foreach (LokiLabel globalLabel in _globalLabels)
                    stream.Labels.Add(new LokiLabel(globalLabel.Key, globalLabel.Value));

                foreach (KeyValuePair<string, LogEventPropertyValue> property in logEvent.Properties)
                    // Some enrichers pass strings with quotes surrounding the values inside the string,
                    // which results in redundant quotes after serialization and a "bad request" response.
                    // To avoid this, remove all quotes from the value.
                    stream.Labels.Add(new LokiLabel(property.Key, property.Value.ToString().Replace("\"", "")));

                var localTime = DateTime.Now;
                var localTimeAndOffset = new DateTimeOffset(localTime, TimeZoneInfo.Local.GetUtcOffset(localTime));
                var time = localTimeAndOffset.ToString("o");

                var sb = new StringBuilder();
                sb.AppendLine(logEvent.RenderMessage());
                if (logEvent.Exception != null)
                {
                    var e = logEvent.Exception;
                    while (e != null)
                    {
                        sb.AppendLine(e.Message);
                        sb.AppendLine(e.StackTrace);
                        e = e.InnerException;
                    }
                }

                stream.Entries.Add(new BatchEntry(time, sb.ToString()));
            }

            if (content.Streams.Count > 0)
                output.Write(content.Serialize());
        }

        public void Format(IEnumerable<string> logEvents, TextWriter output) { return; }

        private int GetLevelRestriction(LogEventLevel eventLevel)
        {
            return eventLevel switch
                   {
                       LogEventLevel.Verbose     => 0,
                       LogEventLevel.Debug       => 1,
                       LogEventLevel.Information => 2,
                       LogEventLevel.Warning     => 3,
                       LogEventLevel.Error       => 4,
                       LogEventLevel.Fatal       => 5,
                       _                         => 2
                   };
        }

        private static string GetLevel(LogEventLevel level)
        {
            if (level == LogEventLevel.Information)
                return "info";

            return level.ToString().ToLower();
        }
    }
}