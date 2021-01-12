using System.Collections.Generic;

namespace Solari.Sol.Tests.TestSetup
{
    public static class ConfigurationCollection
    {
        private static IDictionary<string, string> NoApplicationSection()
            => new Dictionary<string, string>();

        private static IDictionary<string, string> NoApplicationName()
            => new Dictionary<string, string>()
            {
                {"Application:ApplicationName", ""},
                {"Application:Project", "Solari"}

            };

        private static IDictionary<string, string> NoProjectName()
            => new Dictionary<string, string>()
            {
                {"Application:Project", ""},
                {"Application:ApplicationName", "Solari.Sol.Tests"}
            };


        public static IDictionary<string, string> Select(ConfigurationKeys key)
            => key switch
               {
                   ConfigurationKeys.NoApplicationName    => NoApplicationName(),
                   ConfigurationKeys.NoApplicationSection => NoApplicationSection(),
                   ConfigurationKeys.NoProjectName        => NoProjectName(),
                   _                                      => new Dictionary<string, string>()
               };
    }
}
