using System.Collections.Generic;

namespace Solari.Titan.Abstractions
{
    public class OverridesOptions
    {
        public string Microsoft { get; set; } = TitanConstants.GlobalOverrideLevel;
        public string System { get; set; } = TitanConstants.GlobalOverrideLevel;
        public string MicrosoftHostingLifetime { get; set; } = TitanConstants.GlobalOverrideLevel;
        public List<string> Custom { get; set; } = new List<string>();
        public string AspNetCore { get; set; } = TitanConstants.GlobalOverrideLevel;
    }
}