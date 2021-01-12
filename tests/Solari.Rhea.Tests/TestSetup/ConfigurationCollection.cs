using System.Collections.Generic;
using System.Diagnostics;

namespace Solari.Rhea.Tests.TestSetup
{
    internal static class ConfigurationCollection
    {
        private static IDictionary<string, string> AppSettings
            => new Dictionary<string, string>()
            {
                {"Application:ApplicationName", "Solari.Rhea.Tests"},
                {"Application:Project", "Solari"}
            };

        public static IDictionary<string, string> Select(ConfigurationKeys key)
            => key switch
               {
                   ConfigurationKeys.DefaultAppSettings => AppSettings,
                   _                                    => new Dictionary<string, string>()
               };
    }
}
