using System.Collections.Generic;
using Serilog.Sinks.Loki.Labels;

namespace Solari.Titan.Abstractions
{
    public class LokiLabelProvider : ILogLabelProvider
    {
        private readonly string _appName;
        private readonly string _env;

        public LokiLabelProvider(string appName, string env)
        {
            _appName = appName;
            _env = env;
        }

        public IList<LokiLabel> GetLabels()
        {
            return new List<LokiLabel>
            {
                new LokiLabel("app", _appName),
                new LokiLabel("env", _env)
            };
        }
    }
}